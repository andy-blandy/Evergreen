using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void InteractAction();
    public static event InteractAction OnInteract;

    public delegate void BackAction();
    public static event BackAction OnBack;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnInteract != null)
                OnInteract();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnBack != null)
                OnBack();
        }
    }
}
