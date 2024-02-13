using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script to move the bullet and then destroy it after a set amount of time
public class BulletMove : MonoBehaviour
{

    private Rigidbody rb;
    private float speed = 15f;
    private float AliveTime = 0.00f;
    private int dmg = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;

        AliveTime += Time.deltaTime;
        if (AliveTime >= 5.00f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player");
            other.gameObject.GetComponent<PlayerHealth>().Damage(dmg);
        }

        Destroy(gameObject);
    }
}
