/*
 * This is attached to a hitbox that appears whenever the player attacks
 * It looks for any object implementing the Damageable interface and... damages it.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IDamageable>(out IDamageable id))
        {
            // This gets the damage and knockback from the PlayerAttack script
            int damage = Player.instance.playerAttack.damage;
            float knockbackAmount = Player.instance.playerAttack.knockbackAmount;

            id.Damage(damage);

            // Modifies the amount of knockback applied to the enemy based on the distance away from the enemy
            float distanceAway = (collider.transform.position - transform.position).magnitude;
            float distModifier = 1 - (distanceAway / 1.4f);

            id.Knockback(transform.forward * knockbackAmount * distModifier);
        }
    }
}
