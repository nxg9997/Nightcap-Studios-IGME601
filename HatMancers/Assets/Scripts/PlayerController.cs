using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Code Source: https://forum.unity.com/threads/proper-velocity-based-movement-101.462598/
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float jumpForce;

    private Rigidbody body;

    private float vertical;
    private float horizontal;
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") > 0 && grounded)
        {
            body.AddForce(transform.up * jumpForce);
        }
        Vector3 velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
        velocity.y = body.velocity.y;
        body.velocity = velocity;
        transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            grounded = true;
        }
    }

    void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }
}
