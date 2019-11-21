using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
/// <summary>
/// A UI button that sets a player preference
/// Authors: Michael
/// </summary>
public class PlayerPrefButton : MonoBehaviour
{
    // Modifiabl Attributes
    public PrefType prefType;
    public string prefName;
    public int sceneIndex = -1;
    public string newValue;

    // Private Scripts
    private UnityAction methodOnClick;
    private Button thisButton;

    // Private Booleans
    private bool onClickSet = false;

    //  PrefType enum
    public enum PrefType
    {
        Integer,
        Floating,
        Str
    };

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(onClick);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Remove()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetButtonText(string newText)
    {
        Text buttonText = gameObject.GetComponentInChildren<Text>();
        buttonText.text = newText;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetPosition(float x, float y)
    {
        // Getting the Rect Transform component
        RectTransform thisRect = GetComponent<RectTransform>();

        // Changing the rectangle size
        thisRect.anchoredPosition = new Vector2(x, y);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetOnClick(UnityAction method)
    {
        onClickSet = true;
        methodOnClick = method;
    }

    /// <summary>
    /// 
    /// </summary>
    private void setPref()
    {
        // IF either the preference name or the new value is null / empty...
        if (string.IsNullOrEmpty(prefName) || string.IsNullOrEmpty(newValue))
        {
            Debug.LogError("\"prefName\" and/or \"newValue\" are not set!");
        }
        //ELSE... (preference name & new value are set)
        else
        {
            switch (prefType)
            {
                case PrefType.Integer:
                    PlayerPrefs.SetInt(prefName, int.Parse(newValue));
                    break;
                case PrefType.Floating:
                    PlayerPrefs.SetFloat(prefName, float.Parse(newValue));
                    break;
                case PrefType.Str:
                    PlayerPrefs.SetString(prefName, newValue);
                    break;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    private string getPref()
    {
        // Defining the results variables
        string result = "";

        // IF the preference name is null / empty...
        if (string.IsNullOrEmpty(prefName))
        {
            Debug.LogError("\"prefName\" is not set!");
        }
        // ELSE... (the preference name is set)
        else
        {
            switch (prefType)
            {
                case PrefType.Integer:
                    result = PlayerPrefs.GetInt(prefName) + " (Int)";
                    break;
                case PrefType.Floating:
                    result = PlayerPrefs.GetFloat(prefName) + " (Float)";
                    break;
                case PrefType.Str:
                    result = PlayerPrefs.GetString(prefName) + " (String)";
                    break;
            }
        }

        // Returning the result
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    private void onClick()
    {
        // Setting the Player Preference
        setPref();
        // Debug.Log(getPref());
        
        // IF the on-click has been set, call it
        if (onClickSet)
            methodOnClick();

        // IF the scene index has been set, go to that scene
        if (sceneIndex > -1)
            SceneManager.LoadScene(sceneIndex);
    }
}
