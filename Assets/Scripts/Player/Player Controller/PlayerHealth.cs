/*
 * Written by Andrew
 * @andy_blandy
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int startingHealth = 3;
    public int maxHealth = 3;
    public int health { get; set; }
    public Transform spawn;

    private Rigidbody rb;

    public AudioSource damageSFX;

    public static PlayerHealth instance;
    void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        health = startingHealth;
        UpdateHealthUI();
    }

    public void Damage(int damage)
    {
        health -= damage;
        UpdateHealthUI();

        damageSFX.Play();

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

        UpdateHealthUI();
    }

    public void Kill()
    {
        // Respawn player
        Debug.Log("Player has died");
        health = maxHealth;
        UpdateHealthUI();

        if (spawn != null)
        {
            transform.position = spawn.position;
        }
    }

    public void UpdateHealthUI()
    {
        if (PlayerHUD.instance != null)
        {
            PlayerHUD.instance.UpdateHealth(health);
        }
    }

    public void Knockback(Vector3 knockback)
    {
        rb.AddForce(knockback, ForceMode.Impulse);
    }
}
