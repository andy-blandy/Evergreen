using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttack : MonoBehaviour
{
    public RangedEnemy rangedEnemy;

    public void Attack()
    {
        rangedEnemy.Fire();
    }
}
