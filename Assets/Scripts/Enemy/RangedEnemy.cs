using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [SerializeField, Range(0f, 40f)]
    private float followRange = 20f;
    [SerializeField, Range(0f, 30f)]
    private float attackRange = 15f;
    [SerializeField, Range(0f, 30f)]
    private float fleeRange = 3f;

    private Vector3 playerPos;

    //variables for the shooting portion
    [Header("Shooting")]
    [SerializeField]
    private GameObject BulletSpawn;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float fireRate = 2f;
    private float timedBullets;



    void Start()
    {
        agent.stoppingDistance = attackRange;
    }

    void Update()
    {
        timedBullets += Time.deltaTime;

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
        }
        else if (distanceFromPlayer > attackRange)
        {
            Follow();
        }
        else if (distanceFromPlayer < fleeRange) //Flee range is the range where the enemy will flee from the player, green gizmo
        {
            Reposition();
        }
        else
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

        LookAtPlayer();
        agent.destination = playerPos;
    }

    void Attack()
    {
        LookAtPlayer();
        agent.isStopped = true;
        /*
         * Insert attack behavior here
         */
        if (timedBullets > fireRate)
        {
            timedBullets = 0;
            Instantiate(projectile, BulletSpawn.transform.position, gameObject.transform.rotation);
        }
    }

    //back away from the player
    void Reposition()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        //Bunch of code that i couldnt get to work, i triple checked the math and it seems to be ok but the agent did not want to move to the destination
        /*Debug.Log("Running away");

        Vector3 directionToPlayer = transform.position - Player.instance.transform.position;
        Vector3 fleeDirection = transform.position + directionToPlayer;
        Vector3 fleePosition = new Vector3(fleeDirection.x, 1.07f, fleeDirection.z);
        //Vector3 fleeDirection = new Vector3(-Player.instance.transform.position.x, 1, -Player.instance.transform.position.z);
        agent.destination = fleePosition;*/

    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere showing follow range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);

        // Draw a red sphere showing attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw a green sphere showing flee range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fleeRange);
    }
}
