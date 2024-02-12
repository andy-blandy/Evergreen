using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyBase
{
    [Header("Attack")]
    public int damage = 1;
    public float knockbackAmount;

    [Header("Movement")]
    public float timeBetweenMovements = 1f;
    public float _moveTimer;

    public int currentPhase;

    void OnCollisionEnter(Collision other)
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

    protected override void EnemyUpdate()
    {
        _moveTimer += Time.deltaTime;

        switch (currentPhase)
        {
            case 0:
                LookAtPlayer();
                break;
            case 1:
                break;
        }

        if (_moveTimer >= timeBetweenMovements)
        {
            NextPhase();
            _moveTimer = 0;
        }
    }

    void NextPhase()
    {
        currentPhase++;

        if (currentPhase == 1) 
        {
            Move();
        }

        if (currentPhase > 1)
        {
            currentPhase = 0;
            Debug.Log("RESET PATH");
            agent.ResetPath();
        }
    }

    void Move()
    {
        agent.SetDestination(Player.instance.transform.position);
    }

    public override void Damage(int damage)
    {
        health -= damage;

        // Set slime to move almost immediately after recovering from stun
        currentPhase = 0;
        _moveTimer = timeBetweenMovements - 0.1f;

        if (health <= 0)
        {
            Kill();
        }
    }
}
