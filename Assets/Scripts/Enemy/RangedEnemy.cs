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
    public float bulletTimer;

    // Audio
    public AudioSource shootSFX;

    protected override void EnemyUpdate()
    {
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
        bulletTimer += Time.deltaTime;

        LookAtPlayer();
        agent.isStopped = true;

        if (bulletTimer > fireRate)
        {
            shootSFX.Play();
            bulletTimer = 0;
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

        Vector3 fleeDirection = Vector3.Normalize(transform.position - Player.instance.transform.position);
        Vector3 fleePosition = transform.position + fleeDirection * (fleeRange + 1f);
        agent.destination = fleePosition;
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
