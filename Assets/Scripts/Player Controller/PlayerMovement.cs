/*
 * Written by Andrew
 * @andy_blandy
 */

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

    public AudioSource backgroundMusic;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        backgroundMusic.Play();
    }

    /*
     * In update we get the player's input and set the desired velocity of the player based on that input
     */
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
    }
    
    /*
     * In fixed update we move the player by getting the player's current velocity and moving it towards the desired velocity.
     * The input and movement are separated like this since it's recommended to handle any physics-based events in fixed update.
     */
    void FixedUpdate()
    {
        velocity = rb.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        rb.velocity = velocity;
    }
}
