using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles all inputs from the player. Currently consists of:
///     - Movement
///     - Jumping
/// Authors: Abhi, David
/// Code Source: https://forum.unity.com/threads/proper-velocity-based-movement-101.462598/
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Modifiable Attributes
    public Transform head;
    public float speed;
    public float rotationSpeed;
    public float jumpForce;

    // Rigidbody for physics
    private Rigidbody body;

    // Store CameraMouse to access orbit dampening
    private CameraMouse cameraMouse;

    // Stored values for axes
    private float vertical;
    private float horizontal;

    // Flags
    private bool grounded;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        cameraMouse = head.gameObject.GetComponentInChildren<CameraMouse>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove) return;

        // Get axes values
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        // Check if player wants to jump and can jump
        if (Input.GetAxis("Jump") > 0 && grounded)
        {
            body.AddForce(transform.up * jumpForce);
        }

        // Create own gravity for rigidbody
        Vector3 velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
        velocity += (transform.right * horizontal) * speed * Time.fixedDeltaTime;
        velocity.y = body.velocity.y;
        body.velocity = velocity;

        // Set horizontal rotation of the body to the horizontal rotation of the head
        Quaternion QT = Quaternion.Euler(transform.rotation.y, cameraMouse.localRotation.x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * cameraMouse.orbitDampening);
    }

    // Check for when a collider hits this game object
    void OnCollisionEnter (Collision collision)
    {
        // Check if hitting the ground (Layer 8)
        if (collision.gameObject.layer == 8)
        {
            grounded = true;
        }
    }

    // Check for when a collider no longer overlaps this game object
    void OnCollisionExit (Collision collision)
    {
        // Check if leaving the ground (Layer 8)
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }
}
