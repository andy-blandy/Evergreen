using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public bool inTrigger;

    void OnEnable()
    {
        PlayerInput.OnInteract += OpenShop;
    }

    void OnDisable()
    {
        PlayerInput.OnInteract -= OpenShop;
    }

    void OpenShop()
    {
        if (inTrigger)
        {
            Shop.instance.EnterShop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        inTrigger = false;
    }
}
