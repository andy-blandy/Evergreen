using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public CinemachineVirtualCamera roomCamera;
    public GameObject northDoorway, southDoorway, westDoorway, eastDoorway;

    public void OnEnable()
    {
        PlayerInput.OnAlt += SwitchCameraActive;

        EnterRoom();
    }

    public void OnDisable()
    {
        PlayerInput.OnAlt += SwitchCameraActive;
    }

    public void EnterRoom()
    {
        roomCamera.gameObject.SetActive(DungeonManager.instance.isRoomCameraEnabled);
    }


    /// <summary>
    /// Returns the doorway corresponding to the direction given in string form.
    /// </summary>
    /// <param name="direction">"north", "south", "east", "west"</param>
    /// <returns>gameObject or null</returns>
    public GameObject GetDoorway(string direction)
    {
        switch (direction)
        {
            case "north":
                return northDoorway;
            case "south":
                return southDoorway;
            case "east":
                return westDoorway;
            case "west":
                return eastDoorway;
            default:
                Debug.LogAssertion(direction + " is not a valid direction");
                return null;
        }
    }

    public bool SetCameraTarget()
    {
        if (Player.instance != null)
        {
            roomCamera.Follow = Player.instance.transform;
            return true;
        }

        return false;
    }

    public void SwitchCameraActive()
    {
        if (roomCamera == null)
        {
            return;
        }

        roomCamera.gameObject.SetActive(!roomCamera.gameObject.activeSelf);
        DungeonManager.instance.isRoomCameraEnabled = roomCamera.gameObject.activeSelf;
    }
}
