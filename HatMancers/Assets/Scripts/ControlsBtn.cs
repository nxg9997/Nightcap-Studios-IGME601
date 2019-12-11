using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsBtn : MonoBehaviour
{
    public GameObject displayControls;

    // Start is called before the first frame update
    void Start()
    {
        Button self = GetComponent<Button>();
        self.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        displayControls.SetActive(true);
        //Application.Quit();
    }
}
