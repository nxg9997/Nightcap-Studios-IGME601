using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Contains the game's general data.
/// 
/// Authors:
/// - Michael Berger
/// 
/// Sources:
/// - https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
/// </summary>
public class GameData : MonoBehaviour
{
    // Modifiable Attributes
    public int playerCount;
    public GameModes currentMode;
    public bool Player1UsingKeyboard;

    // Enumeration for Game Mode
    public enum GameModes {
        FreeForAll,
        Payload
    };


    /// <summary>
    /// Deletes duplicate GameData objects + Makes sure this GameData object doesn't get deleted on load
    /// </summary>
    void Awake()
    {
        // Deleting this instance of the GameData object if one already exists
        GameObject[] gameDatas = GameObject.FindGameObjectsWithTag("GameData");

        // IF there is already a GameData object...
        if (gameDatas.Length > 1)
        {
            // Destroy this GameData object
            Destroy(this.gameObject);
        }
        // ELSE... (this is the only GameData object)
        else
        {
            // Ensuring this instance of the GameData object doesn't get destroyed on load
            DontDestroyOnLoad(this.gameObject);

            // Setting the Scene Loaded method to be called
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSceneLoaded(Scene sc, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + sc.name);
    }
}
