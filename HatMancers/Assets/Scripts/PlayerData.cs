using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Contains data about the player's current hat/magic, spawn information, etc.
    [SerializeField]
    private GameObject currHat;

    public string currMagic = "none";

    public int health = 100;

    public GameObject spawn;
    private float spawnTimer = 0;
    public float spawnTime = 3;
    public bool dead = false;

    public float killPlaneDepth = -50;

    public GameObject opponent;
    public bool testDummy = false;

    private PlayerController pCtrl;
    private CapsuleCollider cCollider;
    private GameObject hatPosition;

    public bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        // Dummies have certain components removed since they are not needed
        if (testDummy)
        {
            Destroy(GetComponentInChildren<Camera>());
            Destroy(GetComponentInChildren<PlayerController>());
        }

        pCtrl = GetComponent<PlayerController>();
        cCollider = GetComponent<CapsuleCollider>();

        if(pCtrl.playerNum == 1)
        {
            opponent = GameObject.Find("Player2");
        }
        else if (pCtrl.playerNum == 2)
        {
            opponent = GameObject.Find("Player1");
        }

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach(Transform g in children)
        {
            if(g.gameObject.name == "HatLocation")
            {
                hatPosition = g.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            // Allow devs to kill a player by pressing F1 while the player object is in debug mode
            if (Input.GetKeyDown(KeyCode.F1))
            {
                health = 0;
            }
        }

        // Prevent player movement if they are dead
        pCtrl.canMove = !dead;
        cCollider.enabled = !dead;

        // If the player if dead, check if they can respawn
        if (dead)
        {
            Respawn();
        }
        else // Otherwise, do everything else
        {
            StickyHat();
            CheckKillPlane();
            CheckHealth();
        }
    }

    /// <summary>
    /// Checks to see if the player falls below the map, and kills them if they do
    /// </summary>
    void CheckKillPlane()
    {
        if(transform.position.y <= killPlaneDepth)
        {
            health = 0;
        }
    }

    /// <summary>
    /// Updates the current magic type if the player replaces their hat
    /// </summary>
    void UpdateMagic()
    {
        if(currHat.tag == "FireHat")
        {
            currMagic = "fire";
        }
        else if (currHat.tag == "LightningHat")
        {
            currMagic = "lightning";
        }
        else if (currHat.tag == "IceHat")
        {
            currMagic = "ice";
        }
    }

    /// <summary>
    /// Keeps the current hat attached to the player's head
    /// </summary>
    void StickyHat()
    {
        if (currHat == null) return;
        currHat.transform.position = hatPosition.transform.position;
        currHat.transform.rotation = hatPosition.transform.rotation;
    }

    /// <summary>
    /// Kills the player if their health falls to/below 0
    /// </summary>
    void CheckHealth()
    {
        if(health <= 0)
        {
            dead = true;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// If the player is dead, respawn the player after a set amount of time
    /// </summary>
    void Respawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnTime)
        {
            dead = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            //transform.position = spawn.transform.position;
            health = 100;
            spawnTimer = 0;

            transform.position = GameObject.Find("Manager").GetComponent<RespawnManager>().FindSpawnPoint(opponent);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // Updates the current hat if one is picked up
        if (col.gameObject.tag.Contains("Hat"))
        {
            if(currHat != null)
            {
                Destroy(currHat);
            }
            currHat = col.gameObject;
            currHat.GetComponent<Collider>().enabled = false;
            UpdateMagic();
        }
        else if(col.gameObject.tag == "Damage") // Damages the player if they are hit by a damaging source (denoted by the "Damage" tag)
        {
            if(col.gameObject.GetComponent<SpellData>().origin.name != gameObject.name)
                health -= col.gameObject.GetComponent<SpellData>().damage;
            //Debug.Log(health);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Damage") // Damages the player if they are hit by a damaging source (denoted by the "Damage" tag)
        {
            if (col.gameObject.GetComponent<SpellData>().origin.name != gameObject.name)
                health -= col.gameObject.GetComponent<SpellData>().damage;
            //Debug.Log(health);
        }
    }
}
