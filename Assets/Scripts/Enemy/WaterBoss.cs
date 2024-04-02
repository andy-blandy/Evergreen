using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBoss : EnemyBase
{

    public Animator animator;

    [Header("UI")]
    public Slider healthBar;

    [Header("Beaver Summons")]
    public GameObject beaverPrefab;
    public List<Vector3> beaverSpawnpoints;
    public float timeBetweenBeaverSummons;
    public float beaverSummonTimer;

    [Header("Movement Waypoints")]
    public List<Vector3> waypoints;
    public float waypointGizmoSize = 1f;
    public Vector3 currentWaypoint;

    [Header("Healing")]
    public float healingRadius = 2f;
    public List<Transform> healingBarrels;
    public float healTimer;
    public float timeBetweenHeals = 2f;

    [Header("Idle")]
    public float timeIdling = 10f;

    [Header("Attack")]
    public float attackRadius = 5f;
    public float attackTimer;
    public float timeBetweenAttacks = 2f;

    [Header("Collision Damage")]
    public int damage = 1;
    public float knockbackAmount = 5f;

    [Header("Phases")]
    public float phaseTimer;
    public int currentPhase;

    private int idle = 0;
    private int moving = 1;

    void Start()
    {
        beaverSummonTimer = timeBetweenBeaverSummons;
        SummonBeavers();

        healthBar.maxValue = startingHealth;
        healthBar.value = startingHealth;
    }

    public override void Knockback(Vector3 knockback)
    {
        return;
    }

    protected override void EnemyUpdate()
    {
        // Update healthbar
        healthBar.value = health;

        beaverSummonTimer -= Time.deltaTime;
        if (beaverSummonTimer < 0)
        {
            beaverSummonTimer = timeBetweenBeaverSummons;
            SummonBeavers();
        }

        phaseTimer -= Time.deltaTime;
        switch (currentPhase)
        {
            case 0:
                OnGuard();
                break;
            case 1:
                MoveToWaypoint();
                break;
        }
    }

    private void OnGuard()
    {
        LookAtPlayer();
        Healing();
        
        // Switch phase if timer is low enough
        if (phaseTimer < 0)
        {
            GetRandomWaypoint();
            currentPhase = moving;
        }

        // TO-DO: Attack player if they are close enough
        float distFromPlayer = (Player.instance.transform.position - transform.position).magnitude;
        if (distFromPlayer < attackRadius && !animator.GetBool("playerInFront"))
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                FrontAttack();
                attackTimer = timeBetweenAttacks;
            }
        }
    }
    
    private void Healing()
    {
        // Detect if the enemy is close enough to a barrel
        // If they are, slowly heal the enemy
        foreach (Transform t in healingBarrels)
        {
            if (!t.gameObject.activeSelf)
            {
                continue;
            }

            if ((t.position - transform.position).magnitude < healingRadius)
            {
                healTimer -= Time.deltaTime;
                if (healTimer < 0)
                {
                    Heal(1);
                    healTimer = timeBetweenHeals;
                }
            }
        }
    }

    private void FrontAttack()
    {
        animator.SetBool("playerInFront", true);
    }

    private void GetRandomWaypoint()
    {
        // Create list of all possible waypoint options (excluding the waypoint the enemy is currently at)
        List<Vector3> availWaypoints = new List<Vector3>();
        foreach (Vector3 wp in waypoints)
        {
            if (wp.Equals(currentWaypoint))
            {
                continue;
            }

            availWaypoints.Add(wp);
        }

        // Choose a random waypoint from the available options
        int choice = Random.Range(0, availWaypoints.Count);
        currentWaypoint = availWaypoints[choice];
    }

    private void MoveToWaypoint()
    {
        // Update animation
        if (!animator.GetBool("moving"))
        {
            animator.SetBool("moving", true);
        }

        // Move towards the currently selected waypoint
        agent.SetDestination(currentWaypoint);

        // Check if the enemy is close enough to the selected waypoint
        bool atDestination = false;
        if ((transform.position - currentWaypoint).magnitude < 1f)
        {
            atDestination = true;
        }

        // Switch phase to idle phase once the enemy has arrived at the destination
        if (atDestination)
        {
            animator.SetBool("moving", false);
            currentPhase = idle;
            phaseTimer = timeIdling;
            healTimer = timeBetweenHeals;
        }
    }

    private void SummonBeavers()
    {
        int numOfBeaversToSpawn = Random.Range(1, 5);

        for (int i = 0; i < numOfBeaversToSpawn; i++)
        {
            // Choose a random spawnpoint
            int randomSpawn = Random.Range(0, beaverSpawnpoints.Count);
            Instantiate(beaverPrefab, beaverSpawnpoints[randomSpawn], Quaternion.identity);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("COLLIDE");
            IDamageable id = other.gameObject.GetComponent<IDamageable>();

            id.Damage(damage);

            Vector3 playerDirection = other.transform.position - transform.position;
            playerDirection.y = 0;
            playerDirection = Vector3.Normalize(playerDirection);
            Vector3 knockbackVector = playerDirection * knockbackAmount;
            id.Knockback(knockbackVector);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 waypoint in waypoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(waypoint, waypointGizmoSize);
        }

        foreach (Vector3 enemySpawn in beaverSpawnpoints)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(enemySpawn, waypointGizmoSize);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, healingRadius);
    }
}
