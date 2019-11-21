using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static bool isGamePaused = false;
    public static bool resumingGame = false;
    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
