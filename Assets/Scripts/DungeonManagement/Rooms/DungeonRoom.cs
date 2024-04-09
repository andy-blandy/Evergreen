using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public CinemachineVirtualCamera roomCamera;
    public GameObject northLockedDoor, southLockedDoor, westLockedDoor, eastLockedDoor;
    public List<GameObject> enemies;

    [Header("Room Variables")]
    public bool[] isDoorway = new bool[4];
    public bool allEnemiesDefeated;

    [Header("Room Settings")]
    public bool lockRoomOnEnter;
    public bool unlockRoomWhenEnemiesCleared;


    public virtual void EnterRoom()
    {
        //roomCamera.gameObject.SetActive(NewDungeonManager.instance.isRoomCameraEnabled);
        PlayerInput.OnAlt += SwitchCameraActive;
        EnemyBase.OnDeath += ReviewEnemies;

        if (lockRoomOnEnter && !allEnemiesDefeated)
        {
            LockRoom(true);
        }

        ActivateEnemies(true);
    }

    public void ExitRoom()
    {
        roomCamera.gameObject.SetActive(false);
        PlayerInput.OnAlt -= SwitchCameraActive;
        EnemyBase.OnDeath -= ReviewEnemies;

        ActivateEnemies(false);
    }

    public void SpawnEnemies()
    {
        // TO-DO: Add code to spawn new enemies

        allEnemiesDefeated = false;
    }

    public void ActivateEnemies(bool active)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Debug.Log(enemies[i]);
            enemies[i].gameObject.SetActive(active);
        }
    }

    private void ReviewEnemies()
    {
        for (int i = enemies.Count - 1; i > 0; i--)
        {
            if (enemies[i].GetComponent<EnemyBase>().isDefeated)
            {
                enemies.RemoveAt(i);
            }
        }

        if (enemies.Count == 0)
        {
            allEnemiesDefeated = true;

            if (unlockRoomWhenEnemiesCleared)
            {
                LockRoom(false);
            }
        }
    }

    public void KillAllEnemies()
    {
        for (int i = 0;i < enemies.Count; i++)
        {
            EnemyBase enemyScript = enemies[i].GetComponent<EnemyBase>();

            // Make sure enemy hasn't been killed already
            if (enemyScript.isDefeated)
            {
                continue;
            }

            enemyScript.Kill();
        }
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
                return northLockedDoor;
            case "south":
                return southLockedDoor;
            case "east":
                return eastLockedDoor;
            case "west":
                return westLockedDoor;
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
                return northLockedDoor;
            case 1:
                return southLockedDoor;
            case 2:
                return westLockedDoor;
            case 3:
                return eastLockedDoor;
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
        NewDungeonManager.instance.isRoomCameraEnabled = roomCamera.gameObject.activeSelf;
    }

    /// <summary>
    /// Locks room if true is passed through, unlocks room otherwise
    /// </summary>
    /// <param name="setActive"></param>
    public void LockRoom(bool isLocking)
    {
        for (int i = 0; i < 4; i++)
        {
            if (isDoorway[i])
            {
                GetDoorway(i).SetActive(isLocking);
            }
        }
    }

    #region Room Gen
    public void CreateBarrier(int dir, bool hasDoor)
    {
        isDoorway[dir] = hasDoor;

        // Get the pos and rotation of each barrier
        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.identity;
        Vector3 scale = new Vector3(30, 10, 30);
        float doorSize = 3f;

        switch (dir)
        {
            case 0:
                pos.z = pos.z + 15;
                scale.z = 1;
                rot = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 1:
                pos.z = pos.z - 15;
                scale.z = 1;
                break;
            case 2:
                pos.x = pos.x - 15;
                scale.x = 1;
                rot = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 3:
                pos.x = pos.x + 15;
                scale.x = 1;
                rot = Quaternion.Euler(0f, 270f, 0f);
                break;
        }

        /*
         * Spawn trees
         */
        GameObject trees = CreateTreeLine(15f);
        pos.y = 0;
        trees.transform.position = pos;
        trees.transform.rotation = rot;

        /*
         * Create the invisible borders of each room
         */
        pos.y = 5;
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

            for (int i = 0; i < 5; i++)
            {
                trees.transform.GetChild(i).gameObject.SetActive(false);
            }
        }


    }

    public GameObject CreateTreeLine(float length)
    {
        GameObject treeLine = new GameObject();
        treeLine.name = "Trees";
        treeLine.transform.SetParent(this.transform);

        /*
         * Spawn trees in a straight line
         */
        GameObject tree = RoomGeneration.instance.barrierTree;
        int numOfTrees = Mathf.RoundToInt(length / 2.5f);
        GameObject firstTree = Instantiate(tree, new Vector3(0f, 0f, 0.5f), Quaternion.identity);
        firstTree.transform.parent = treeLine.transform;
        for (int i = 1; i < numOfTrees; i++)
        {
            float newX = 0;
            float newZ = 0;

            if (i % 2 == 0)
            {
                newZ = 0.5f;
            } else
            {
                newZ = 2.5f;
            }

            newX = 2.5f * i;

            Vector3 newPos = new Vector3(newX, 0f, newZ);
            GameObject newTreeRight = Instantiate(tree, newPos, Quaternion.identity);
            newPos.x *= -1;
            GameObject newTreeLeft = Instantiate(tree, newPos, Quaternion.identity);

            // Add trees to tree line
            newTreeLeft.transform.SetParent(treeLine.transform);
            newTreeRight.transform.SetParent(treeLine.transform);
        }

        return treeLine;
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
    #endregion
}
