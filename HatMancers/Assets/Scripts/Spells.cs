using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public GameObject fireProj;
    public float fireForce = 5.0f;
    private bool fireDelay = false;
    private float fireTime = 0.0f;
    public float fireDelayTime = 2;

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
        if (Input.GetAxis("Fire1") > 0 && !fireDelay && pData.currMagic == "fire")
        {
            Fire();
            fireDelay = true;
            fireTime = 0;
        }
        else
        {
            fireTime += Time.deltaTime;
            if(fireTime > fireDelayTime)
            {
                fireDelay = false;
            }
        }
    }

    void Fire()
    {
        GameObject fireball = GameObject.Instantiate(fireProj, transform.position + transform.forward * 2, Quaternion.identity);
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
}
