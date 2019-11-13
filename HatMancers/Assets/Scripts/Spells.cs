using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    // Fire Data
    public GameObject fireProj;
    public float fireForce = 5.0f;
    private bool fireDelay = false;
    private float fireTime = 0.0f;
    public float fireDelayTime = 2;
    public Vector3 fireOffset;

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

    // UI Color
    public Color UIColor_Fire;
    public Color UIColor_Ice;
    public Color UIColor_Lightning;

    // Player Scripts
    private PlayerController pController;
    private PlayerData pData;

    // Other
    private int playerNum;
    private GameObject spellOrigin;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        pController = GetComponent<PlayerController>();
        pData = GetComponent<PlayerData>();

        Transform[] trans = GetComponentsInChildren<Transform>();
        foreach(Transform t in trans)
        {
            if(t.gameObject.name == "SpellOrigin")
            {
                spellOrigin = t.gameObject;
            }

            else if(t.gameObject.name == "Main Camera")
            {
                cam = t.gameObject.GetComponent<Camera>();
            }
        }

        // Setting the spell charge time variables
        fireTime = fireDelayTime;
        lightningTime = lightningDelayTime;

        // Getting the player number
        playerNum = pController.GetPlayerNum();
    }

    // Update is called once per frame
    void Update()
    {
        // Only check input if the player is human-controlled
        if (!pData.testDummy)
        {
            CheckInput();
        }
    }

    /// <summary>
    /// Use magic based on the current hat equipped & if the player clicks the shoot button
    /// </summary>
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

    /// <summary>
    /// Shoots a fireball if the fire hat is equipped. Fireballs can only be launched after a set interval.
    /// </summary>
    void Fire()
    {
        // Create fireball and set its rotation
        GameObject fireball = GameObject.Instantiate(fireProj, spellOrigin.transform.position + fireOffset + spellOrigin.transform.forward * 2, Quaternion.identity);
        fireball.GetComponent<SpellData>().origin = gameObject;
        fireball.transform.forward = cam.transform.forward;
        /*Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
        foreach(Transform t in trans)
        {
            if(t.gameObject.name == "HatLocation")
            {
                fireball.transform.forward = t.forward;//gameObject.GetComponentInChildren<Camera>().transform.forward;
                break;
            }
        }*/
        
        // Shoot fireball
        fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * fireForce);
    }

    /// <summary>
    /// Creates a lightning bolt (sniper shot) that gets deleted after a set period of time. A raycast is used to detect a hit.
    /// </summary>
    void Lightning()
    {
        GameObject bolt = GameObject.Instantiate(lightningObj);
        bolt.GetComponent<SpellData>().origin = gameObject;
        Transform[] trans = bolt.GetComponentsInChildren<Transform>();
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;
        RaycastHit rch;

        Transform[] trans2 = GetComponentsInChildren<Transform>();
        /*Transform hatTrans = transform;
        foreach (Transform t in trans2)
        {
            if(t.gameObject.name == "HatLocation")
            {
                hatTrans = t;
                break;
            }
        }*/

        foreach (Transform t in trans)
        {
            if (t.gameObject.name.Contains("Start"))
            {
                t.position = spellOrigin.transform.position + transform.forward * 2;
                start = t.position;
            }
            else if (t.gameObject.name.Contains("End"))
            {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rch))
                {
                    t.position = rch.point;
                    end = t.position;
                }
                else
                {
                    t.position = spellOrigin.transform.position + (cam.transform.forward * lightningDist);
                    end = t.position;
                }
            }
        }
        lightningPersistTimer = 0;
        lightningTime = 0;
        currBolt = bolt;

        bool hit = Physics.Raycast(cam.transform.position, cam.transform.forward, out rch, lightningDist);
        if(hit)
        {
            Debug.Log("hit object " + rch.collider.gameObject.name);
            if (rch.collider.gameObject.name.Contains("Player"))
            {
                rch.collider.gameObject.GetComponent<PlayerData>().health -= bolt.GetComponent<SpellData>().damage;
            }
        }
    }

    /// <summary>
    /// An ice "flamethrower" is sprayed in front of the player (close range) that deals damage over time.
    /// </summary>
    void Ice()
    {
        if(currIce == null)
        {
            currIce = GameObject.Instantiate(iceObj, transform.position + transform.forward * 1.5f, transform.rotation);
            currIce.GetComponent<SpellData>().origin = gameObject;
        }
        else
        {
            currIce.transform.position = transform.position + transform.forward * 1.5f;
            currIce.transform.rotation = transform.rotation;
        }
    }

    /// <summary>
    /// Returns the charge percent of the current spell. (1.0 = ready, 0.0 = full charge needed)
    /// </summary>
    public float SpellChargePercent()
    {
        // Defining the result value
        float result = 0;

        // SWITCH for the current magic...
        switch (pData.currMagic)
        {
            case "fire":
                result = (fireTime / fireDelayTime);
                break;
            case "lightning":
                result = (lightningTime / lightningDelayTime);
                break;
            case "none":
            case "ice":
                result = 1;
                break;
            // (Undefined Magic)
            default:
                Debug.LogError("You have not defined the \"" + pData.currMagic + "\" case in SpellChargePercent()!");
                break;
        }

        // Clamping the result
        result = Mathf.Clamp(result, 0, 1);

        // Returning the result
        return result;
    }

    /// <summary>
    /// Returns the UI color of the spell.
    /// </summary>
    public Color SpellColorUI()
    {
        // Defining the result color
        Color result = new Color(255, 255, 255, 1);

        // SWITCH for the current magic...
        switch (pData.currMagic)
        {
            case "none":
                break;
            case "fire":
                result = UIColor_Fire;
                break;
            case "lightning":
                result = UIColor_Lightning;
                break;
            case "ice":
                result = UIColor_Ice;
                break;
            default:
                Debug.LogError("You have not defined the \"" + pData.currMagic + "\" case in SpellColorUI()!");
                break;
        }

        // Returning the result color
        return result;
    }
}
