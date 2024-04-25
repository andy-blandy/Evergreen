using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DamageableBarrel : MonoBehaviour, IDamageable
{
    public int startingHealth = 3;
    public int health { get; set; }

    [Header("Components")]
    private Rigidbody rb;
    public float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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

    public void Knockback(Vector3 knockback)
    {
        return;
    }
}
