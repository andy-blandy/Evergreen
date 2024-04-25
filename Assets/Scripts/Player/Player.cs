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

    [Header("Input")]
    public bool isFrozen;
    public bool inShop;

    void Awake()
    {
        instance = this;

        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetXP(int value)
    {
        xp = value;

        if (PlayerHUD.instance != null)
        {
            PlayerHUD.instance.UpdateXP(xp);
        }
    }

    public void FreezePlayer(bool freeze)
    {
        isFrozen = freeze;
        
        // Stops all of the player's velocity, but not when the game is paused
        if (freeze && Time.timeScale > 0.0f)
        {
            playerMovement.ClearVelocity();
        }
    }

}
