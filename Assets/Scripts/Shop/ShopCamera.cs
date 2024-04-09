using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCamera : MonoBehaviour
{
    public List<GameObject> cameras = new List<GameObject>();
    private int activeCameraIndex;
    
    public void ActivateCameras()
    {
        cameras[0].SetActive(true);
        activeCameraIndex = 0;
    }

    public void DeactivateCameras()
    {
        cameras[activeCameraIndex].SetActive(false);
    }

    public void SwitchCamera(int newCamera)
    {
        if (newCamera > cameras.Count)
        {
            Debug.LogAssertion("Camera doesn't exist!");
            return;
        }

        cameras[newCamera].SetActive(true);
        cameras[activeCameraIndex].SetActive(false);
        activeCameraIndex = newCamera;
    }
    
    public int GetActiveCamera()
    {
        return activeCameraIndex;
    }
}
