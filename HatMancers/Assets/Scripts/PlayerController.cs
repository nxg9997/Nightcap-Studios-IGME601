using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float jumpHoldTime; // Time the button can be held down to add force into the air
    public float gravityForce; // Force of gravity weighing down in air

    // Rigidbody for physics
    private Rigidbody body;

    // Store CameraMouse to access orbit dampening
    private CameraMouse cameraMouse;

    // Storing WizardAnimScript to control animations
    private WizardAnimScript animScript;

    // Stored values for axes
    private float vertical;
    private float horizontal;

    // Timers
    private float jumpTimer;

    // Flags
    private bool grounded;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        jumpTimer = 0;
        body = GetComponent<Rigidbody>();
        cameraMouse = head.gameObject.GetComponentInChildren<CameraMouse>();
        animScript = this.gameObject.GetComponentInChildren<WizardAnimScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!canMove) return;

        // Get axes values
        vertical = Input.GetAxis("LSV" + playerNum);
        horizontal = Input.GetAxis("LSH" + playerNum);

        // Check if player wants to jump and can jump
        if (Input.GetAxis("A" + playerNum) > 0 && grounded)
        {
            // Set the vertical velocity immediately
            //Vector3 newVelocity = body.velocity;
            //newVelocity.y = jumpSpeed;
            //body.velocity = newVelocity;
            body.AddForce(Vector3.up * jumpSpeed, ForceMode.Force);
            jumpTimer = 0;
        }
        else if (Input.GetAxis("A" + playerNum) != 0 && !grounded && jumpTimer < jumpHoldTime)
        {
            body.AddForce(Vector3.up * jumpSpeed, ForceMode.Force);
            jumpTimer += Time.deltaTime;
        }
        else if (Input.GetAxis("A" + playerNum) == 0 && !grounded)
        {
            // Set counter force
            body.AddForce(Vector3.down * gravityForce, ForceMode.Force);
        }
        else if (!grounded)
        {
            body.AddForce(Vector3.down * (gravityForce / 2f), ForceMode.Force);
        }

        // Add more speed only if it hasn't reached max speed
        if (body.velocity.x < speed)
            body.AddForce(transform.forward * vertical * speed, ForceMode.Force);
        if (body.velocity.z < speed)
            body.AddForce(transform.right * horizontal * speed, ForceMode.Force);

        // Deadzone for velocity
        if (body.velocity.magnitude < 0.1)
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

        //Vector3 velocity = (transform.forward * vertical) * speed * Time.deltaTime;
        //velocity += (transform.right * horizontal) * speed * Time.deltaTime;
        //velocity.y = body.velocity.y;
        //body.velocity += velocity;

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
}
