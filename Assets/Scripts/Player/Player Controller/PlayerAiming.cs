/*
 * Written by Andrew
 * @andy_blandy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public Vector3 lookPosition;
    public Vector3 look;

    [Range(0f, 100f)] public float lookSpeed = 10f;

    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = Player.instance.playerMovement;
    }


    /*
     * Find the position of the mouse in the world, and use that to find where the player should look at
     */
    void Update()
    {
        if (Player.instance.isFrozen)
        {
            return;
        }

        if (playerMovement.isDashing)
        {
            lookPosition = playerMovement.endOfDashPos;
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            lookPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        }

        look = new Vector3(lookPosition.x, transform.position.y, lookPosition.z);
    }

    /*
     * Rotates the player
     */
    void FixedUpdate()
    {
        Quaternion lookRot = Quaternion.LookRotation(look - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, lookSpeed * Time.deltaTime);
    }
}
