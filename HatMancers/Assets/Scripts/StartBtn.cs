using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    // Modifiable Attributes
    public Canvas menuCanvas;
    public QuitBtn quit;
    public ControlsBtn controls;
    public GameObject prefButtonPrefab;
    public Text controllerText;
    public int sceneIndex;

    // Private Scripts
    private RectTransform startRect;
    private Vector3 originalPosition;
    private float startOriginalWidth = 0f;
    private float startOriginalHeight = 0f;

    // Controller variables
    private string[] controllers;
    private bool setControllers = false;

    // Start is called before the first frame update
    void Start()
    {
        startRect = GetComponent<RectTransform>();
        startOriginalWidth = startRect.rect.width;
        startOriginalHeight = startRect.rect.height;
        Button self = GetComponent<Button>();
        self.onClick.AddListener(OnClick);
        originalPosition = transform.position;
        controllers = new string[4];
    }

    private void Update()
    {
        if (setControllers)
        {
            // Check EVERY controller input
            if (Input.GetButtonDown("STA1"))
                HandleControllerStart("Keyboard");
            else if (Input.GetButtonDown("STA2"))
                HandleControllerStart("Xbox1");
            else if (Input.GetButtonDown("STA3"))
                HandleControllerStart("Xbox2");
            else if (Input.GetButtonDown("STA4"))
                HandleControllerStart("Xbox3");
            else if (Input.GetButtonDown("STA5"))
                HandleControllerStart("Xbox4");
            else if (Input.GetButtonDown("STA6"))
                HandleControllerStart("PS1");
            else if (Input.GetButtonDown("STA7"))
                HandleControllerStart("PS2");
            else if (Input.GetButtonDown("STA8"))
                HandleControllerStart("PS3");
            else if (Input.GetButtonDown("STA9"))
                HandleControllerStart("PS4");
        }
    }

    private void HandleControllerStart(string name)
    {
        // Remove input if in the controllers already
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] == name)
            {
                controllers[i] = null;
                UpdateControllerText();
                return;
            }
        }

        // Add to controllers if there is room
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] == null)
            {
                controllers[i] = name;
                UpdateControllerText();
                return;
            }
        }
    }

    private void UpdateControllerText()
    {
        controllerText.text = "\n\n";

        for (int i = 1; i < controllers.Length + 1; i++)
        {
            controllerText.text += "Player " + i + ": ";

            if (controllers[i - 1] != null)
                controllerText.text += controllers[i - 1];

            controllerText.text += "\n";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private PlayerPrefButton[] GetPrefButtons()
    {
        return FindObjectsOfType<PlayerPrefButton>();
    }

    /// <summary>
    /// Deleting the Preference Button objects
    /// </summary>
    private void DeletePrefButtons(PlayerPrefButton[] toDelete)
    {
        for (int num = 0; num < toDelete.Length; num++)
        {
            // Destroy(toDelete[num]);
            toDelete[num].Remove();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnClick()
    {
        /*
        //Debug.Log("testing");
        SceneManager.LoadScene(1);
        Cursor.visible = false;
        */

        // Hiding the Start and Quit buttons
        startRect.position = new Vector3(-9999, -9999, 0);
        controls.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        controllerText.enabled = true;

        // Set up controller variables
        setControllers = true;
        for (int i = 0; i < controllers.Length; i++)
            controllers[i] = null;

        // Showing the Player Count preferences
        SetPlayerNumbers();
    }

    /// <summary>
    /// 
    /// </summary>
    void ShowMainMenu()
    {
        // Getting the leftover Preference Buttons
        PlayerPrefButton[] leftoverPrefs = GetPrefButtons();

        // Remove controllers
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = null;
        }
        UpdateControllerText();

        // Setting the Start and Quit buttons to be active
        //startRect.rect.Set(startRect.rect.x, startRect.rect.y, startOriginalWidth, startOriginalHeight);
        startRect.position = originalPosition;
        controls.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        controllerText.enabled = false;

        // Deleting the leftover Preference Buttons
        DeletePrefButtons(leftoverPrefs);
    }

    void StartGame()
    {
        // Get controller count
        int controllerCount = 0;
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] != null)
                controllerCount++;
        }

        // Don't allow start of the game with less than 2 players
        if (controllerCount <= 1)
            return;

        // Set variables to carry into next scene
        for (int i = 1; i <= controllers.Length; i++)
        {
            if (controllers[i - 1] != null)
            {
                PlayerPrefs.SetString("Player" + i + "Controller", controllers[i - 1]);
            }
        }

        // Set amount of players
        PlayerPrefs.SetInt("PlayerCount", controllerCount);

        // Load game
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    void SetPlayerNumbers()
    {
        // Creating "Method Static" variables
        string playerCount = "PlayerCount";

        // Getting the leftover Preference Buttons
        PlayerPrefButton[] leftoverPrefs = GetPrefButtons();

        // Creating the four preference buttons
        GameObject[] prefButtonObjects = new GameObject[2];
        PlayerPrefButton[] prefButtonScripts = new PlayerPrefButton[2];
        RectTransform[] prefButtonRects = new RectTransform[2];
        for (int num = 0; num < 2; num++)
        {
            prefButtonObjects[num] = Instantiate(prefButtonPrefab);
            prefButtonScripts[num] = prefButtonObjects[num].GetComponent<PlayerPrefButton>();
            prefButtonRects[num] = prefButtonObjects[num].GetComponent<RectTransform>();

        }

        /*// Setting "2 Players" button
        prefButtonObjects[0].transform.SetParent(menuCanvas.transform, false);
        prefButtonScripts[0].SetButtonText("2 Players");
        prefButtonScripts[0].prefType = PlayerPrefButton.PrefType.Integer;
        prefButtonScripts[0].prefName = playerCount;
        prefButtonScripts[0].newValue = "2";
        prefButtonScripts[0].sceneIndex = 1;

        // Setting the "3 Players" button
        prefButtonObjects[1].transform.SetParent(menuCanvas.transform, false);
        prefButtonScripts[1].SetPosition(prefButtonRects[1].rect.x,
                                        prefButtonRects[1].rect.y - 75f);
        prefButtonScripts[1].SetButtonText("3 Players");
        prefButtonScripts[1].prefType = PlayerPrefButton.PrefType.Integer;
        prefButtonScripts[1].prefName = playerCount;
        prefButtonScripts[1].newValue = "3";
        prefButtonScripts[1].sceneIndex = 1;

        // Setting the "4 Players" button
        prefButtonObjects[2].transform.SetParent(menuCanvas.transform, false);
        prefButtonScripts[2].SetPosition(prefButtonRects[2].rect.x,
                                        prefButtonRects[2].rect.y - 150f);
        prefButtonScripts[2].SetButtonText("4 Players");
        prefButtonScripts[2].prefType = PlayerPrefButton.PrefType.Integer;
        prefButtonScripts[2].prefName = playerCount;
        prefButtonScripts[2].newValue = "4";
        prefButtonScripts[2].sceneIndex = 1;*/

        // Setting the "Back" button
        prefButtonObjects[0].transform.SetParent(menuCanvas.transform, false);
        prefButtonScripts[0].SetPosition(prefButtonRects[0].rect.x - 125f,
                                        prefButtonRects[0].rect.y - 100f);
        prefButtonScripts[0].SetButtonText("Back");
        prefButtonScripts[0].prefType = PlayerPrefButton.PrefType.Integer;
        prefButtonScripts[0].prefName = playerCount;
        prefButtonScripts[0].newValue = "1";
        prefButtonScripts[0].SetOnClick(ShowMainMenu);

        // Setting the "Start" button
        prefButtonObjects[1].transform.SetParent(menuCanvas.transform, false);
        prefButtonScripts[1].SetPosition(prefButtonRects[1].rect.x + 275f,
                                        prefButtonRects[1].rect.y - 100f);
        prefButtonScripts[1].SetButtonText("Start");
        prefButtonScripts[1].SetOnClick(StartGame);


        // Deleting the leftover Preference Buttons
        DeletePrefButtons(leftoverPrefs);
    }
}
