/*
 * Written by Andrew
 * @andy_blandy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int startingHealth = 3;
    public int maxHealth = 3;
    public int health { get; set; }
    public Transform spawn;

    void Start()
    {
        health = startingHealth;
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
    }

    public void Heal(int healamount)
    {
        if ((health + healamount) > maxHealth)
        {
            health = maxHealth;
        } else
        {
            health += healamount;
        }
    }

    public void Kill()
    {
        Debug.Log("Player has died");
        health = maxHealth;
        transform.position = spawn.position;
        /*
         * Insert code to respawn player
         */
    }
}
