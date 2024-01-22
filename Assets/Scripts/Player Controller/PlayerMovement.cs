using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0f, 100f)] public float maxSpeed;
    [Range(0f, 100f)] public float maxAcceleration;

    private Rigidbody rb;
    public Vector2 playerInput;
    public Vector3 velocity, desiredVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
    }
    
    void FixedUpdate()
    {
        velocity = rb.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        rb.velocity = velocity;
    }
}
