using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float knockbackAmount;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IDamageable>(out IDamageable id))
        {
            id.Damage(1);

            float distanceAway = (collider.transform.position - transform.position).magnitude;
            Debug.Log("Distance: " + distanceAway);

            id.Knockback(transform.forward * knockbackAmount);
        }
    }
}
