using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DamageableBarrel : MonoBehaviour, IDamageable
{
    public int startingHealth = 3;
    public int health { get; set; }

    public TextMeshProUGUI healthText;

    public UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    public float speed;

    void Start()
    {
        health = startingHealth;

        // Create a new NavMeshAgent.
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Set the agent's speed.
        agent.speed = 10f;
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
        agent.destination = target.position;
    }
    // void OnTriggerStay(Collider collider)
    // {
    //     if(collider.tag == "Player")
    //     {
    //         agent.destination = target.transform.position;
    //     }
    // }
}
