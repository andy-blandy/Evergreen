using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : EnemyBase
{
    [SerializeField, Range(0f, 20f)]
    private float followRange = 10f;
    [SerializeField, Range(0f, 20f)]
    private float attackRange = 3f;

    private Vector3 playerPos;

    void Start()
    {
        agent.stoppingDistance = attackRange;
    }

    void Update()
    {
        // Updates the stopping distance of the NavMeshAgent if the attack range is changed while in the unity editor
        #if UNITY_EDITOR
        if (agent.stoppingDistance != attackRange)
        {
            agent.stoppingDistance = attackRange;
        }
        #endif

        if (isStunned)
        {
            Stunned();
            return;
        }

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

        /*
         * Insert idle behavior here
         */
    }

    void Follow()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }


        Debug.Log("FOLLOWING");
        LookAtPlayer();
        agent.destination = playerPos;
    }

    void Attack()
    {
        LookAtPlayer();

        /*
         * Insert attack behavior here
         */
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
