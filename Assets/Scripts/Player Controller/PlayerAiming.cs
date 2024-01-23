/*
 * Written by Andrew
 * @andy_blandy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public Vector3 mouseWorldPosition;
    public Vector3 look;

    /*
     * Find the position of the mouse in the world, and use that to find where the player should look at
     */
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        look = new Vector3(mouseWorldPosition.x, transform.position.y, mouseWorldPosition.z);
    }

    /*
     * Rotates the player
     */
    void FixedUpdate()
    {
        transform.LookAt(look);
    }
}
