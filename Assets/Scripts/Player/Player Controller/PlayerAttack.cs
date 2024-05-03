using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;

    public float minimumKnockbackAmount = 5f;
    public float knockbackAmount = 10f;

    public float attackLength;
    public Coroutine attackCoroutine;
    bool isAttacking;

    [Header("Punches")]
    public bool chargingLeft;
    public bool chargingRight;

    public Animator upperBodyAnimator;

    public AudioSource attackSFX;

    void Start()
    {
        isAttacking = false;
    }

    void Update()
    {
        if (Player.instance.isFrozen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {            
            // Audio
            attackSFX.Play();
            Attack();
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            // Choose a random arm to punch with
            float choice = Random.Range(0f, 1f);
            if (choice < 0.5f)
            {
                upperBodyAnimator.SetBool("leftPunch", true);
            } else
            {
                upperBodyAnimator.SetBool("leftPunch", false);
            }

            // Audio
            attackSFX.Play();
        }

        upperBodyAnimator.SetTrigger("Punch");
    }

    // 1 = left punch, 0 = right punch
    public void SetNextPunch(int punchChoice)
    {
        if (punchChoice == 1)
        {
            upperBodyAnimator.SetBool("leftPunch", true);
        }
        else if (punchChoice == 0)
        {
            upperBodyAnimator.SetBool("leftPunch", false);
        }
    }

    public void SetAttacking(int num)
    {
        if (num == 0)
        {
            isAttacking = false;
        }
        else if (num == 1)
        {
            isAttacking = true;
        }
    }
}
