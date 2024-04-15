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

    public AudioSource damageSFX;
    public AudioSource deathSFX;

    [Header("Visuals")]
    public List<Material> slimeMaterials;
    public MeshRenderer meshRenderer;

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

    protected override void EnemyAwake()
    {
        int randInt = Random.Range(0, slimeMaterials.Count - 1);

        meshRenderer.material = slimeMaterials[randInt];
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
            agent.ResetPath();
        }
    }

    void Move()
    {
        deathSFX.Play();
        agent.SetDestination(Player.instance.transform.position);
    }

    public override void Damage(int damage)
    {
        health -= damage;

        damageSFX.Play();

        // Set slime to move almost immediately after recovering from stun
        currentPhase = 0;
        _moveTimer = timeBetweenMovements - 0.1f;

        if (health <= 0)
        {
            damageSFX.Play();
            Kill();
        }
    }
}
