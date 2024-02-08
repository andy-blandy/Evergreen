using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.instance.xp++;
            PlayerHUD.instance.UpdateXP(Player.instance.xp);
            gameObject.SetActive(false);
        }
    }
}
