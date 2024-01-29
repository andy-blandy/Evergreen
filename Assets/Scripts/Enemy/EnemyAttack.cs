using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float nextTime;
    public float timeBetweenAttacks;

    public MeshRenderer meshRenderer;
    private YieldInstruction waitToHideAttack;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        waitToHideAttack = new WaitForSeconds(0.4f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            nextTime = Time.time + timeBetweenAttacks * 0.5f;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
        {
            return;
        }

        if (Time.time > nextTime) {
            if (collider.TryGetComponent<IDamageable>(out IDamageable id))
            {
                Debug.Log("ATTACK");
                // Display attack visual
                meshRenderer.enabled = true;
                StartCoroutine(HideAttack());
                id.Damage(1);
               
            }
            nextTime = Time.time + timeBetweenAttacks;
        }
    }

    IEnumerator HideAttack()
    {
        yield return waitToHideAttack;
        meshRenderer.enabled = false;

    }
}
