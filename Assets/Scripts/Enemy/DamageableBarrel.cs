using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DamageableBarrel : MonoBehaviour, IDamageable
{
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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        health = startingHealth;
    }

    void Update()
    {
        healthText.text = health.ToString();

        if (isStunned)
        {
            Stunned();
            return;
        }

        agent.destination = target.position;
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        Vector3 lookVector = target.position - transform.position;
        lookVector.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed);
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

    // void OnTriggerStay(Collider collider)
    // {
    //     if(collider.tag == "Player")
    //     {
    //         agent.destination = target.transform.position;
    //     }
    // }
}
