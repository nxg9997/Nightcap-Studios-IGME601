using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLeave : MonoBehaviour
{

    private GameObject bounds;

    // Start is called before the first frame update
    void Start()
    {
        bounds = GameObject.Find("ArenaBounds");
    }

    // Update is called once per frame
    void Update()
    {
        if (bounds.GetComponent<ArenaBounds>().CheckBoundsSphere(transform.position))
        {
            Destroy(gameObject);
        }
    }
}
