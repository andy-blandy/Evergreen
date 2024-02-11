using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;

    public int health { get; set; }

    [Header("Stun")]
    public float stunLength = 1f;
    protected float stunTimer;
    public bool isStunned;

    [Header("Movement")]
    [Range(0f, 0.5f)]
    public float turnSpeed = 0.2f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Damage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Kill();
        }
    }

    public virtual void Heal(int healamount)
    {
    }

    public virtual void Kill()
    {
        gameObject.SetActive(false);
        XPManager.instance.SpawnXP(transform.position);
    }

    /*
     * Applies a force to the enemy and stuns them for the duration of the float stunLength
     * This function is called from the AttackHitbox script within the Player Controller folder
     */
    public virtual void Knockback(Vector3 knockback)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        agent.isStopped = true;
        rb.AddForce(knockback, ForceMode.Impulse);
        isStunned = true;
        stunTimer = stunLength;
    }

    /*
     * Counts down the stunTimer and prevent the enemy from moving for the duration of it
     */
    protected virtual void Stunned()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer < 0)
        {
            // Remove any forces being applied to the rigidbody
            rb.velocity = Vector3.zero;

            agent.isStopped = false;
            isStunned = false;
        }
    }

    /*
     * Rotates the transform of the enemy towards the player
     * Uses the turnSpeed variable
     */
    protected virtual void LookAtPlayer()
    {
        Vector3 lookVector = Player.instance.transform.position - transform.position;
        lookVector.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed);
    }
}
