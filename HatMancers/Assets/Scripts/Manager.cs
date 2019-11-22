using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Modifiable Attributes
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public bool debug = false;
    public static Manager instance;
    public static bool isGamePaused = false;
    public static bool resumingGame = false;
    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // Getting the starting player count
        int startingPlayers = PlayerPrefs.GetInt("PlayerCount", 4);

        // SWITCH for the amount of players
        switch (startingPlayers)
        {
            case 2:
                // Removing the last 2 Wizard Players + changing both camera spaces
                if (debug)
                {
                    Debug.Log("Manager.Start(): Case 2 flipped");
                }
                else
                {
                    // Destroying the unused players
                    if (player3 != null)
                        Destroy(player3);
                    if (player4 != null)
                        Destroy(player4);

                    // Setting the remaining player's camera spaces
                    Camera playerCamera1 = player1.GetComponentInChildren<Camera>();
                    Camera playerCamera2 = player2.GetComponentInChildren<Camera>();
                    playerCamera1.rect = new Rect(0f, 0.5f, 1f, 0.5f);
                    playerCamera2.rect = new Rect(0f, 0f, 1f, 0.5f);
                }
                break;
            case 3:
                // Removing the last Wizard Player + changing Wizard Player 3's camera
                if (debug)
                {
                    Debug.Log("Manager.Start(): Case 3 flipped");
                }
                else
                {
                    // Destroying the unused players
                    Destroy(player4);

                    // Setting the remaining player's camera spaces
                    Camera playerCamera3 = player3.GetComponentInChildren<Camera>();
                    playerCamera3.rect = new Rect(0f, 0f, 1f, 0.5f);
                }

                break;
            case 4:
                if (debug)
                    Debug.Log("Manager.Start(): Case 4 flipped");
                // ELSE, do nothing (game will proceed with 4 players)
                break;
            default:
                Debug.LogError(startingPlayers + "-player arenas are currently not supported!");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {       
        // If player presses Escape button/Start button
/*        if (Input.GetKeyDown(KeyCode.Escape))//(Input.GetAxis("STA" + GameObject.Find("Player1").GetComponent<PlayerController>().GetPlayerNum()) > 0)
        {
            if (!isGamePaused)
            {
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }*/
    }

    // Load up the Pause Menu UI
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Resume the play, when Resume button is pressed on the Pause Menu UI
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        resumingGame = true;
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
        Time.timeScale = 1f;
        isGamePaused = false;
        resumingGame = true;
        SceneManager.LoadScene(0);
    }
}
