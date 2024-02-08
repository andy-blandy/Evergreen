/*
 * This script a singleton for enemies and other classes to reference the player object
 * Also contains methods to modify the player's stats, as well as keep track of XP
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Script References")]
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;

    [Header("XP")]
    public int xp;

    void Awake()
    {
        instance = this;

        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        PlayerHUD.instance.UpdateXP(xp);
    }

    #region Methods to Increase/Decrease Stats
    public void IncreaseMaxHealth(int increase)
    {
        playerHealth.maxHealth += increase;
    }

    public void IncreaseDashLength(float increase)
    {
        playerMovement.dashLength += increase;
    }

    public void DecreaseDashCooldown(float decrease)
    {
        if (playerMovement.dashCooldown - decrease > 0)
        {
            playerMovement.dashCooldown -= decrease;
        }
    }

    public void IncreaseDamage(int increase)
    {
        playerAttack.damage += increase;
    }

    public void IncreaseKnockback(float increase)
    {
        playerAttack.knockbackAmount += increase;
    }
    #endregion

}
