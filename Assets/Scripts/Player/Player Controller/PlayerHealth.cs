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

    public bool isDead;

    public Animator animator;

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
        animator.SetTrigger("Respawn");
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
        if (!isDead)
        {
            Player.instance.FreezePlayer(true);
            animator.ResetTrigger("Respawn");
            animator.SetTrigger("Die");
            isDead = true;
        }
    }

    void Respawn()
    {
        health = startingHealth;
        UpdateHealthUI();

        if (spawn != null)
        {
            transform.position = spawn.position;

            Player.instance.FreezePlayer(false);
            animator.ResetTrigger("Die");
            animator.SetTrigger("Respawn");
            isDead = false;

            // Reset the player's upgrades
            UpgradeManager.instance.ResetUpgrades();

            // Randomize the dungeon
            NewDungeonManager.instance.GenerateNewDungeon();
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
