using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;
    public float knockbackAmount = 25f;

    public float attackLength;
    public GameObject attackHitbox;
    public Coroutine attackCoroutine;
    public bool isAttacking;

    public AudioSource attackSFX;

    void Start()
    {
        isAttacking = false;
        attackHitbox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackAnimation());

            // Audio
            attackSFX.Play();
        }
    }

    /*
     * This will probably be changed once we have a character model with attack animations implemented
     * Activates the attack hitbox for the attack length
     */
    IEnumerator AttackAnimation()
    {
        isAttacking = true;
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackLength);

        isAttacking = false;
        attackHitbox.SetActive(false);
    }
}
