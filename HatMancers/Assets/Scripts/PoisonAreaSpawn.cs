using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAreaSpawn : MonoBehaviour
{
    public GameObject poisonArea;
    public float gravity;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(-(transform.up * gravity));
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Terrain")
        {
            GameObject area = Instantiate(poisonArea, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.Euler(-90,0,0));
            area.GetComponent<SpellData>().origin = gameObject.GetComponent<SpellData>().origin;
            Destroy(gameObject);
        }
    }
}
