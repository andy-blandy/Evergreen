using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstuctionMask;

    public bool canSeePlayer;


    public int startingHealth = 3;
    public int health { get; set; }

    public TextMeshProUGUI healthText;

    [Header("Look")]
    public float turnSpeed = 0.2f;

    [Header("Stun")]
    public float stunAmount;
    public bool isStunned;
    private float stunTimer;

    [Header("Components")]
    private Rigidbody rb;
    public NavMeshAgent agent;
    public Transform target;
    public float speed;
    public float minimumKnockbackAmount = 5f;
    public float knockbackAmount = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        health = startingHealth;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        LookAtPlayer();
    }

    void Update()
    {
        healthText.text = health.ToString();

        if (isStunned)
        {
            Stunned();
            return;
        }
        LookAtPlayer();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle/2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstuctionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            if(canSeePlayer )
            {
                Charge();
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
    
    void LookAtPlayer()
    {
        Vector3 lookVector = target.position - transform.position;
        lookVector.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed);
    }
    void Charge()
    {
        agent.destination = target.position;
    }

    void Stunned()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer < 0)
        {
            agent.isStopped = false;
            isStunned = false;
        }
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable id))
        {
            float minKnockback = Player.instance.playerAttack.minimumKnockbackAmount;
            float knockbackAmount = Player.instance.playerAttack.knockbackAmount;

            float distanceAway = (collision.transform.position - transform.position).magnitude;
            float distModifier = 1 - (distanceAway / 1.4f);

            Vector3 knockbackForce = transform.forward * knockbackAmount * distModifier;
                if (knockbackForce.magnitude < minKnockback)
                {
                    knockbackForce = transform.forward * minKnockback;
                }

                id.Knockback(knockbackForce);

        }
    }
    
    public void Knockback(Vector3 knockback)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        agent.isStopped = true;
        rb.AddForce(knockback, ForceMode.Impulse);
        isStunned = true;
        stunTimer = stunAmount;
    }
}
