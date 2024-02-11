using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void InteractAction();
    public static event InteractAction OnInteract;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnInteract();
        }
    }
}
