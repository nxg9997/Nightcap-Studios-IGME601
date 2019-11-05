using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button self = GetComponent<Button>();
        self.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        Application.Quit();
    }
}
