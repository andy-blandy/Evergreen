using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int value;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.instance.SetXP(Player.instance.xp + value);
            gameObject.SetActive(false);
        }
    }
}
