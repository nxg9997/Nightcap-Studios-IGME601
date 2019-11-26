using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsBackBtn : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject displayControlsUI;

    // Start is called before the first frame update
    void Start()
    {
        Button self = GetComponent<Button>();
        self.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (Manager.isGamePaused)
        {
            displayControlsUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("StartMenu2.0");
        }
        
    }
}
