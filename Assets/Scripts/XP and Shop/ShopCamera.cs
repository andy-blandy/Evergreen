using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCamera : MonoBehaviour
{
    public List<GameObject> cameras = new List<GameObject>();
    private int activeCamera;
    
    public void Enter()
    {
        cameras[0].SetActive(true);
        activeCamera = 0;
    }

    public void Exit()
    {
        cameras[activeCamera].SetActive(false);
    }

    public void SwitchCamera(int newCamera)
    {
        if (newCamera > cameras.Count)
        {
            Debug.LogAssertion("Camera doesn't exist!");
            return;
        }

        cameras[newCamera].SetActive(true);
        cameras[activeCamera].SetActive(false);
        activeCamera = newCamera;
    }
    
    public int GetActiveCamera()
    {
        return activeCamera;
    }
}
