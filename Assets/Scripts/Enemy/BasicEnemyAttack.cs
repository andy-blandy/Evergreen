using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAttack : MonoBehaviour
{
    public int damage = 1;
    public float knockbackAmount = 15f;

    void OnTriggerEnter(Collider other)
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
}
