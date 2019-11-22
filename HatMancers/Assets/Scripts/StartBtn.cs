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
    public GameObject prefButtonPrefab;

    // Private Scripts
    private RectTransform startRect;
    private float startOriginalWidth = 0f;
    private float startOriginalHeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startRect = GetComponent<RectTransform>();
        startOriginalWidth = startRect.rect.width;
        startOriginalHeight = startRect.rect.height;
        Button self = GetComponent<Button>();
        self.onClick.AddListener(OnClick);
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
        startRect.rect.Set(startRect.rect.x, startRect.rect.y, 0, 0);
        quit.gameObject.SetActive(false);

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

        // Setting the Start and Quit buttons to be active
        startRect.rect.Set(startRect.rect.x, startRect.rect.y, startOriginalWidth, startOriginalHeight);
        quit.gameObject.SetActive(true);

        // Deleting the leftover Preference Buttons
        DeletePrefButtons(leftoverPrefs);
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
        GameObject[] prefButtonObjects = new GameObject[4];
        PlayerPrefButton[] prefButtonScripts = new PlayerPrefButton[4];
        RectTransform[] prefButtonRects = new RectTransform[4];
        for (int num = 0; num < 4; num++)
        {
            prefButtonObjects[num] = Instantiate(prefButtonPrefab);
            prefButtonScripts[num] = prefButtonObjects[num].GetComponent<PlayerPrefButton>();
            prefButtonRects[num] = prefButtonObjects[num].GetComponent<RectTransform>();

        }

        // Setting "2 Players" button
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
        prefButtonScripts[2].sceneIndex = 1;

        // Setting the "Back" button
        prefButtonObjects[3].transform.SetParent(menuCanvas.transform, false);
        prefButtonScripts[3].SetPosition(prefButtonRects[3].rect.x,
                                        prefButtonRects[3].rect.y - 225f);
        prefButtonScripts[3].SetButtonText("Back");
        prefButtonScripts[3].prefType = PlayerPrefButton.PrefType.Integer;
        prefButtonScripts[3].prefName = playerCount;
        prefButtonScripts[3].newValue = "1";
        prefButtonScripts[3].SetOnClick(ShowMainMenu);


        // Deleting the leftover Preference Buttons
        DeletePrefButtons(leftoverPrefs);
    }
}
