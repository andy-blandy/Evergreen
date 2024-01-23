using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageableBarrel : MonoBehaviour, IDamageable
{
    public int startingHealth = 3;
    public int health { get; set; }

    public TextMeshProUGUI healthText;

    void Start()
    {
        health = startingHealth;
    }

    public void Damage(int damage)
    {
        health -= damage;

        /*
         * Damage audio would go here
         */

        if (health <= 0)
        {
            Kill();
        }
    }

    public void Heal(int healamount)
    {
    }

    public void Kill()
    {
        /*
         * Death audio would go here
         */

        gameObject.SetActive(false);
    }

    void Update()
    {
        healthText.text = health.ToString();
    }
}
