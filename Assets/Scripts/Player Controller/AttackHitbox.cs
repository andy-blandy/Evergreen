/*
 * This is attached to a hitbox that appears whenever the player attacks
 * It looks for any object implementing the Damageable interface and... damages it.
 */

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
            float distModifier = 1 - (distanceAway / 1.4f);

            id.Knockback(transform.forward * knockbackAmount * distModifier);
        }
    }
}
