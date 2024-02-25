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

        // EnterRoom();
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
                return eastDoorway;
            case "west":
                return westDoorway;
            default:
                Debug.LogAssertion(direction + " is not a valid direction");
                return null;
        }
    }

    /// <summary>
    /// Returns the doorway corresponding to the direction given in string form.
    /// </summary>
    /// <param name="direction">0, 1, 2, 3</param>
    /// <returns>gameObject or null</returns>
    public GameObject GetDoorway(int direction)
    {
        switch (direction)
        {
            case 0:
                return northDoorway;
            case 1:
                return southDoorway;
            case 2:
                return westDoorway;
            case 3:
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

    public void CreateBarrier(int dir, bool hasDoor)
    {
        Vector3 pos = transform.position;
        pos.y = 5;
        Vector3 scale = new Vector3(30, 10, 30);
        float doorSize = 1f;

        switch (dir)
        {
            case 0:
                pos.z = pos.z + 15;
                scale.z = 1;
                break;
            case 1:
                pos.z = pos.z - 15;
                scale.z = 1;
                break;
            case 2:
                pos.x = pos.x - 15;
                scale.x = 1;
                break;
            case 3:
                pos.x = pos.x + 15;
                scale.x = 1;
                break;
        }

        if (!hasDoor)
        {
            CreateCollider(pos, scale);
        }
        else
        {
            Vector3 pos2 = pos;
            switch (dir) 
            {
                case 0:
                case 1:
                    float xMoveAmount = ((0.25f * scale.x) + doorSize);
                    pos.x = pos.x + xMoveAmount;
                    pos2.x = pos2.x - xMoveAmount;
                    scale.x = (0.5f * scale.x) - doorSize;
                    break;
                case 2:
                case 3:
                    float zMoveAmount = ((0.25f * scale.z) + doorSize);
                    pos.z = pos.z + zMoveAmount;
                    pos2.z = pos2.z - zMoveAmount;
                    scale.z = (0.5f * scale.z) - doorSize;
                    break;
            }

            CreateCollider(pos, scale);
            CreateCollider(pos2, scale);
        }


    }

    public GameObject CreateCollider(Vector3 center, Vector3 size)
    {
        GameObject newBarrier = new GameObject();
        newBarrier.name = "wall";
        newBarrier.transform.position = center;
        newBarrier.AddComponent<BoxCollider>();
        newBarrier.transform.localScale = size;
        newBarrier.transform.SetParent(this.transform);

        return newBarrier;
    }
}
