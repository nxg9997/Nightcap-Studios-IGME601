using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private GameObject currHat;

    public string currMagic = "none";

    public int health = 100;

    public GameObject spawn;
    private float spawnTimer = 0;
    public float spawnTime = 3;
    public bool dead = false;

    public float killPlaneDepth = -50;

    public bool testDummy = false;

    // Start is called before the first frame update
    void Start()
    {
        if (testDummy)
        {
            Destroy(GetComponentInChildren<Camera>());
            Destroy(GetComponentInChildren<PlayerController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            Respawn();
        }
        else
        {
            StickyHat();
            CheckKillPlane();
            CheckHealth();
        }
    }

    void CheckKillPlane()
    {
        if(transform.position.y <= killPlaneDepth)
        {
            health = 0;
        }
    }

    void UpdateMagic()
    {
        if(currHat.tag == "FireHat")
        {
            currMagic = "fire";
        }
    }

    void StickyHat()
    {
        if (currHat == null) return;
        currHat.transform.position = GameObject.Find("HatLocation").transform.position;
        currHat.transform.rotation = GameObject.Find("HatLocation").transform.rotation;
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            dead = true;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Respawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnTime)
        {
            dead = false;
            GetComponent<MeshRenderer>().enabled = true;
            transform.position = spawn.transform.position;
            health = 100;
            spawnTimer = 0;
        }
    }

    void OnCollisionEnter(Collision col)
    {
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
        else if(col.gameObject.tag == "Damage")
        {
            health -= col.gameObject.GetComponent<SpellData>().damage;
        }
    }
}
