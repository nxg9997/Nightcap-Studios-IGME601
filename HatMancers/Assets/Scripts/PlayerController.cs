using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Handles all inputs from the player. Currently consists of:
///     - Movement
///     - Jumping
/// Authors: Abhi, David, Michael
/// Code Source: https://forum.unity.com/threads/proper-velocity-based-movement-101.462598/
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Modifiable Attributes
    public int playerNum;
    public Transform head;
    public float speed;
    public float rotationSpeed;
    public float jumpSpeed; // Initial jump velocity
    public float gravityForce; // Force of gravity weighing down in air

    //Pause Menu Variables
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;

    // Rigidbody for physics
    private Rigidbody body;

    // Store CameraMouse to access orbit dampening
    private CameraMouse cameraMouse;

    // Storing WizardAnimScript to control animations
    private WizardAnimScript animScript;

    // Stored values for axes
    private float vertical;
    private float horizontal;

    // Flags
    private bool grounded;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        cameraMouse = head.gameObject.GetComponentInChildren<CameraMouse>();
        animScript = this.gameObject.GetComponentInChildren<WizardAnimScript>();
    }

    // Update is called once per frame
    void Update()
    {

        // If player presses Escape button/Start button
        if (Input.GetKeyDown(KeyCode.Escape))//(Input.GetAxis("STA" + GameObject.Find("Player1").GetComponent<PlayerController>().GetPlayerNum()) > 0)
        {
            if (!isGamePaused)
            {
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // If the game is paused, stop scene activities
        if (isGamePaused)
        {
            return;
        }

        if (!canMove) return;

        // Get axes values
        vertical = Input.GetAxis("LSV" + playerNum);
        horizontal = Input.GetAxis("LSH" + playerNum);

        // Check if player wants to jump and can jump
        if (Input.GetAxis("A" + playerNum) > 0 && grounded)
        {
            // Set the vertical velocity immediately
            Vector3 newVelocity = body.velocity;
            newVelocity.y = jumpSpeed;
            body.velocity = newVelocity;
        }
        else if (Input.GetAxis("A" + playerNum) == 0 && !grounded)
        {
            // Set counter force
            body.AddForce(Vector3.down * gravityForce);
        }

        // Create own gravity for rigidbody
        Vector3 velocity = (transform.forward * vertical) * speed * Time.deltaTime;
        velocity += (transform.right * horizontal) * speed * Time.deltaTime;
        velocity.y = body.velocity.y;
        body.velocity = velocity;

        // Set horizontal rotation of the body to the horizontal rotation of the head
        transform.rotation = Quaternion.Euler(transform.rotation.y, cameraMouse.localRotation.x, 0f);
        //Quaternion QT = Quaternion.Euler(transform.rotation.y, cameraMouse.localRotation.x, 0f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * cameraMouse.orbitDampening);

        /*  ======================
            ===== ANIMATIONS =====
            ======================
        */

        // IF the player is grounded & the player is NOT moving...
        if (grounded && vertical == 0 && horizontal == 0)
        {
            // Playing the idle animation
            animScript.Idle();
        }

        // IF the player is grounded & the player is moving...
        if (grounded && (vertical != 0f || horizontal != 0f))
        {
            // Play the running animation
            animScript.Run();
        }

        // IF the player presses the shoot button...
        if (Input.GetAxis("LT" + playerNum) > 0 || Input.GetAxis("RT" + playerNum) > 0)
        {
            // Play the shooting animation
            animScript.Shoot();
        }
    }

    // Check for when a collider hits this game object
    void OnCollisionEnter (Collision collision)
    {
        // Check if hitting the ground (Layer 8)
        if (collision.gameObject.layer == 8)
        {
            grounded = true;
        }
    }

    // Check for when a collider no longer overlaps this game object
    void OnCollisionExit (Collision collision)
    {
        // Check if leaving the ground (Layer 8)
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }

    /// <summary>
    /// Gets the player number value.
    /// </summary>
    public int GetPlayerNum()
    {
        return playerNum;
    }


    // **************************************************************************************** //
    // **********************************PAUSE MENU FUNCTIONS********************************** //
    // **************************************************************************************** //
    // Load up the Pause Menu UI
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        isGamePaused = true;
    }

    // Resume the play, when Resume button is pressed on the Pause Menu UI
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Maybe display controls on clicking Settings button
    public void LoadSettings()
    {
        Debug.Log("Loading Settings......");
    }

    // Quit the current game and load the landing scene of the Build
    public void QuitMatch()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    // **************************************************************************************** //
    // ********************************PAUSE MENU FUNCTIONS END******************************** //
    // **************************************************************************************** //
}
