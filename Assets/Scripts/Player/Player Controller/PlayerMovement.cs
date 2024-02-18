/*
 * Written by Andrew
 * @andy_blandy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;
    public Vector2 playerInput;
    public Vector3 velocity, desiredVelocity;

    [Header("Movement")]
    [Range(0f, 100f)] public float maxSpeed;
    [Range(0f, 100f)] public float maxAcceleration;
    [Range(0f, 30f)] public float dashSpeed = 20f;

    [Header("Dashing")]
    [Range(1f, 10f)] public float dashLength = 6f;
    [Range(0f, 2f)] public float dashCooldown = 0.5f;
    public bool isDashing;
    public bool dashCooling;
    public Vector3 endOfDashPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /*
     * In update we get the player's input and set the desired velocity of the player based on that input
     */
    void Update()
    {
        // if (Player.instance.isFrozen)
        // {
        //     return;
        // }

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashCooling)
        {
            Dash();
        }
    }
    
    /*
     * In fixed update we move the player by getting the player's current velocity and moving it towards the desired velocity.
     * The input and movement are separated like this since it's recommended to handle any physics-based events in fixed update.
     */
    void FixedUpdate()
    {
        if (isDashing)
        {
            float dashStep = dashSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endOfDashPos, dashStep);

            // Stop dashing when the player is near the goal position
            if ((transform.position - endOfDashPos).magnitude < 0.1f)
            {
                isDashing = false;
                StartCoroutine(CoolDash());
            }

            return;
        }

        velocity = rb.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        rb.velocity = velocity;
    }

    /*
     * Complicated dash function
     * 
     * A problem that exists atm is that an enemy may move in the way of the player while they're dashing, and the player will send the enemy FLYING
     * Will probs need to implement something that stops a dash when the player hits an enemy (and maybe damages the player too...)
     */
    private void Dash()
    {
        // With the player input, we have a vector pointing in the direction the player wants to move
        Vector3 dashVector = new Vector3(playerInput.x, 0f, playerInput.y);

        // We can use a raycast to find any object that might interupt the dash, and stop the player there.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dashVector, out hit, dashLength))
        {
            endOfDashPos = hit.point;

            // Subtract the player's width so they don't run into the object
            endOfDashPos += (dashVector * -1 * transform.localScale.x);
            endOfDashPos.y = transform.position.y;
        } else
        {
            // Otherwise, the player will move the entire length of their dash
            endOfDashPos = transform.position + (dashVector * dashLength);
        }
        isDashing = true;
        dashCooling = true;
    }

    IEnumerator CoolDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashCooling = false;
    }
    public void Knockback(Vector3 knockback)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        rb.AddForce(knockback, ForceMode.Impulse);
    }
}
