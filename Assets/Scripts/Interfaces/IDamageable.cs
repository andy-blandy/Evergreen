using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int health { get; set; }

    void Damage(int damage);
    void Knockback(Vector3 knockback);
    void Heal(int healamount);
    void Kill();
}
