using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Modifiable Attributes
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public List<GameObject> players;
    public bool debug = false;
    public static Manager instance;
    public static bool isGamePaused = false;
    public static bool resumingGame = false;
    public GameObject pauseMenuUI;

    public float matchTimeMin = 5;
    private Text matchTimerText;

    [SerializeField]
    private float matchTimeMs;

    [SerializeField]
    private float timePast = 0;

    public bool matchStarted = true;

    // Start is called before the first frame update
    void Start()
    {
        matchTimeMs = matchTimeMin * 60; // convert to ms
        matchTimerText = GameObject.Find("MatchTimer").GetComponent<Text>();

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

                    // Add active players
                    players.Add(player1);
                    players.Add(player2);
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

                    // Add active players
                    players.Add(player1);
                    players.Add(player2);
                    players.Add(player3);
                }

                break;
            case 4:
                if (debug)
                    Debug.Log("Manager.Start(): Case 4 flipped");
                else
                {
                    // Add active players
                    players.Add(player1);
                    players.Add(player2);
                    players.Add(player3);
                    players.Add(player4);
                }
                break;
            default:
                Debug.LogError(startingPlayers + "-player arenas are currently not supported!");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {       
        if (matchStarted)
        {
            timePast += Time.deltaTime;

            float timer = matchTimeMs - timePast;
            

            if(timePast >= matchTimeMs)
            {
                //game end
                // Get texts of each player and their scores
                List<Text> resultTexts = new List<Text>();
                List<int> playerScores = new List<int>();
                foreach(GameObject player in players)
                {
                    Text[] allText = player.GetComponentsInChildren<Text>();
                    foreach (Text t in allText)
                    {
                        if (t.gameObject.name == "ResultText")
                        {
                            resultTexts.Add(t);
                        }
                    }

                    playerScores.Add(player.GetComponent<PlayerData>().score);
                }

                // Check for winners
                List<int> winnerIndices = new List<int>();
                int winScore = 0;
                for (int i = 0; i < playerScores.Count; i++)
                {
                    if (playerScores[i] > winScore)
                    {
                        winScore = playerScores[i];
                        winnerIndices.Clear();
                        winnerIndices.Add(i);
                    }
                    else if (playerScores[i] == winScore)
                    {
                        winnerIndices.Add(i);
                    }
                }

                // Change texts appropriately
                for (int i = 0; i < resultTexts.Count; i++)
                {
                    if (winnerIndices.Contains(i))
                    {
                        resultTexts[i].text = "WIN";
                    }
                    else
                    {
                        resultTexts[i].text = "LOSE";
                    }
                }

                // Make a delay before going back to title screen
                StartCoroutine(WaitForEnd());
            }
            else
            {
                int minutes = (int)timer / 60;
                int seconds = (int)timer % 60;
                matchTimerText.text = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            }
        }
    }

    // Load up the Pause Menu UI
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        matchTimerText.rectTransform.position = new Vector3(Screen.width / 2f, Screen.height - (matchTimerText.rectTransform.rect.height / 2f));
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
        matchTimerText.rectTransform.position = new Vector3(Screen.width / 2f, Screen.height / 2f);
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
        matchTimerText.rectTransform.position = new Vector3(0, 0, 0);
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene(0);
    }


}
