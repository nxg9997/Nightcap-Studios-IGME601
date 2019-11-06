using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public float matchTimeMin = 5;

    [SerializeField]
    private float matchTimeMs;

    [SerializeField]
    private float timePast = 0;

    public bool matchStarted = false;

    private GameObject p1;
    private GameObject p2;
    // Start is called before the first frame update
    void Start()
    {
        matchTimeMs = matchTimeMin * 60; // convert to ms
        p1 = GameObject.Find("Player1");
        p2 = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (matchStarted)
        {
            timePast += Time.deltaTime;

            float timer = matchTimeMs - timePast;
            

            if(timePast >= matchTimeMs)
            {
                //game end
                Text p1Txt = null;
                Text[] p1AllTxt = p1.GetComponentsInChildren<Text>();
                foreach(Text t in p1AllTxt)
                {
                    if(t.gameObject.name == "ResultText")
                    {
                        p1Txt = t;
                        break;
                    }
                }

                Text p2Txt = null;
                Text[] p2AllTxt = p2.GetComponentsInChildren<Text>();
                foreach (Text t in p2AllTxt)
                {
                    if (t.gameObject.name == "ResultText")
                    {
                        p2Txt = t;
                        break;
                    }
                }

                if(p1.GetComponent<PlayerData>().score > p2.GetComponent<PlayerData>().score)
                {
                    if(p1Txt != null)
                        p1Txt.text = "WIN";

                    if (p2Txt != null)
                        p2Txt.text = "LOSE";
                }
                else if (p1.GetComponent<PlayerData>().score < p2.GetComponent<PlayerData>().score)
                {
                    if (p1Txt != null)
                        p1Txt.text = "LOSE";

                    if (p2Txt != null)
                        p2Txt.text = "WIN";
                }
                else
                {
                    if (p1Txt != null)
                        p1Txt.text = "DRAW";

                    if (p2Txt != null)
                        p2Txt.text = "DRAW";
                }

                StartCoroutine(WaitForEnd());
            }
            else
            {
                int minutes = (int)timer / 60;
                int seconds = (int)timer % 60;
                GameObject.Find("MatchTimer").GetComponent<Text>().text = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            }
        }
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene(0);
    }


}
