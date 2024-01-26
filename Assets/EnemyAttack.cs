using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float nextTime;

    void OnTriggerStay(Collider collider)
    {
        if (Time.time > nextTime) {
            if (collider.TryGetComponent<IDamageable>(out IDamageable id))
            {
                id.Damage(1);
            }
            nextTime = Time.time + 1;
        }
    }
}
