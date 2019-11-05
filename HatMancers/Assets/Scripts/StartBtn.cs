using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
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
        //Debug.Log("testing");
        SceneManager.LoadScene(1);
    }
}
