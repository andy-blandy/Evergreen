using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverEnemy : EnemyBase
{
    [SerializeField, Range(0f, 50f)]
    private float followRange = 40f;
    [SerializeField, Range(0f, 20f)]
    private float attackRange = 3f;

    [Header("Attack")]
    public Animator animator;
    public float attackTimer;
    public float timeBetweenAttacks = 2f;


    private Vector3 playerPos;


    protected override void EnemyUpdate()
    {
        // State controller based on enemy's distance from player
        playerPos = Player.instance.transform.position;
        float distanceFromPlayer = (transform.position - playerPos).magnitude;

        if (distanceFromPlayer > followRange)
        {
            Idle();
        } else if (distanceFromPlayer > attackRange) 
        {
            Follow();
        } else
        {
            Attack();
        }
    }

    void Idle()
    {
        agent.isStopped = true;

        animator.SetBool("walking", false);
    }

    void Follow()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        // Update animation
        if (!animator.GetBool("walking"))
        {
            animator.SetBool("walking", true);
        }

        Debug.Log("FOLLOWING");
        LookAtPlayer();
        agent.destination = playerPos;
    }

    void Attack()
    {
        if (animator.GetBool("walking"))
        {
            animator.SetBool("walking", false);
        }

        LookAtPlayer();

        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            FrontAttack();
            attackTimer = timeBetweenAttacks;
        }
    }

    void FrontAttack()
    {
        animator.SetBool("attack", true);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere showing follow range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);

        // Draw a red sphere showing attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
