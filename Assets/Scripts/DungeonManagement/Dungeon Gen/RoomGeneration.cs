using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public List<GameObject> combatRooms;
    public List<GameObject> shopRooms;
    public GameObject bossDoor;
    public GameObject bossRoom;
    public float roomSize = 3;
    public GameObject barrierTree;

    public static RoomGeneration instance;
    void Awake()
    {
        instance = this;
    }

    public void SpawnRoom(Room room, Vector3 dungeonSpawnPos)
    {
        Vector3 spawnPos = new Vector3(room.xPos * roomSize, 0f, room.yPos * roomSize) + dungeonSpawnPos;
        DungeonRoom newRoom = Instantiate(GetRandomRoom(room), spawnPos, Quaternion.identity).GetComponent<DungeonRoom>();
        room.dungeonRoom = newRoom;
        room.gameObject = newRoom.gameObject;

        // Set doorways
        /*
         * 0 = up
         * 1 = down
         * 2 = left
         * 3 = right
         */
        for (int i = 0; i < 4; i++)
        {

            if (i == 1 && room.pos.Equals(Vector2Int.zero))
            {
                newRoom.CreateBarrier(i, true);
                continue;
            }

            if (!room.adjRooms.ContainsKey(i))
            {
                newRoom.CreateBarrier(i, false);
                continue;
            }
            // newRoom.GetDoorway(i).SetActive(true);
            newRoom.CreateBarrier(i, true);
        }
    }

    public GameObject GetRandomRoom(Room room)
    {
        GameObject chosenRoom = null;

        /*
         * 0 = Combat
         * 1 = Shop
         * 2 = Boss
         */
        int random = -1;
        switch (room.roomType) {
            case 0:
                random = Random.Range(0, combatRooms.Count);
                chosenRoom = combatRooms[random];
                break;
            case 1:
                random = Random.Range(0, shopRooms.Count);
                chosenRoom = shopRooms[random];
                break;
            case 2:
                chosenRoom = bossDoor;
                break;
        
        }

        return chosenRoom;
    }

}
