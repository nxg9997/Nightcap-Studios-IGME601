using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    // Fire Data
    public int playerNum;
    public GameObject fireProj;
    public float fireForce = 5.0f;
    private bool fireDelay = false;
    private float fireTime = 0.0f;
    public float fireDelayTime = 2;

    // Lightning Data
    public GameObject lightningObj;
    private bool lightningDelay = false;
    private float lightningTime = 0.0f;
    public float lightningDelayTime = 5;
    public float lightningDist = 10;
    public float lightningPersistTime = 2;
    public float lightningPersistTimer = 0;
    private GameObject currBolt;

    // Ice Data
    public GameObject iceObj;
    private GameObject currIce;

    // Player Data Script
    private PlayerData pData;

    // Start is called before the first frame update
    void Start()
    {
        pData = GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pData.testDummy)
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        if ((Input.GetAxis("LT" + playerNum) > 0 || Input.GetAxis("RT" + playerNum) > 0) && !fireDelay && pData.currMagic == "fire")
        {
            Fire();
            fireDelay = true;
            fireTime = 0;
        }
        else if ((Input.GetAxis("LT" + playerNum) > 0 || Input.GetAxis("RT" + playerNum) > 0) && !lightningDelay && pData.currMagic == "lightning")
        {
            Lightning();
            lightningDelay = true;
            lightningTime = 0;
        }
        else if ((Input.GetAxis("LT" + playerNum) > 0 || Input.GetAxis("RT" + playerNum) > 0) && pData.currMagic == "ice")
        {
            Ice();
        }
        else
        {
            if(pData.currMagic == "fire")
            {
                fireTime += Time.deltaTime;
                if (fireTime > fireDelayTime)
                {
                    fireDelay = false;
                }
            }
            else if (pData.currMagic == "lightning")
            {
                if(currBolt != null)
                {
                    lightningPersistTimer += Time.deltaTime;
                    if (lightningPersistTimer > lightningPersistTime)
                    {
                        Destroy(currBolt);
                        currBolt = null;
                    }
                }
                
                lightningTime += Time.deltaTime;
                if (lightningTime > lightningDelayTime)
                {
                    lightningDelay = false;
                }
            }
            else if(pData.currMagic == "ice")
            {
                Destroy(currIce);
                currIce = null;
            }

        }
    }

    void Fire()
    {
        GameObject fireball = GameObject.Instantiate(fireProj, transform.position + transform.forward * 2, Quaternion.identity);
        fireball.GetComponent<SpellData>().origin = gameObject;
        Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
        foreach(Transform t in trans)
        {
            if(t.gameObject.name == "Head")
            {
                fireball.transform.forward = t.forward;//gameObject.GetComponentInChildren<Camera>().transform.forward;
                break;
            }
        }
        
        fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * fireForce);
    }

    void Lightning()
    {
        GameObject bolt = GameObject.Instantiate(lightningObj);
        bolt.GetComponent<SpellData>().origin = gameObject;
        Transform[] trans = bolt.GetComponentsInChildren<Transform>();
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;

        Transform hatTrans = transform;
        foreach (Transform t in trans)
        {
            if(t.gameObject.name == "HatLocation")
            {
                hatTrans = t;
                break;
            }
        }

            foreach (Transform t in trans)
        {
            if (t.gameObject.name.Contains("Start"))
            {
                t.position = transform.position + transform.forward * 2;
                start = t.position;
            }
            else if (t.gameObject.name.Contains("End"))
            {
                t.position = hatTrans.transform.position + hatTrans.transform.forward * lightningDist;
                end = t.position;
            }
        }
        lightningPersistTimer = 0;
        lightningTime = 0;
        currBolt = bolt;

        RaycastHit rch;
        bool hit = Physics.Raycast(start, end, out rch, 10);
        if(hit)
        {
            //Debug.Log("hit object");
            if (rch.collider.gameObject.name.Contains("Player"))
            {
                rch.collider.gameObject.GetComponent<PlayerData>().health -= bolt.GetComponent<SpellData>().damage;
            }
        }
    }

    void Ice()
    {
        if(currIce == null)
        {
            currIce = GameObject.Instantiate(iceObj, transform.position + transform.forward * 1.5f, transform.rotation);
        }
        else
        {
            currIce.transform.position = transform.position + transform.forward * 1.5f;
            currIce.transform.rotation = transform.rotation;
        }
    }
}
