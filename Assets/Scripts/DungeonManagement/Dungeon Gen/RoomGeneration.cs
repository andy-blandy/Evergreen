using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public float roomSize = 3;

    public static RoomGeneration instance;
    void Awake()
    {
        instance = this;
    }

    public void SpawnDungeon(List<Room> dungeonRooms)
    {
        foreach (Room room in dungeonRooms)
        {
            SpawnRoom(room);
        }
    }

    public void SpawnRoom(Room room)
    {
        Vector3 spawnPos = new Vector3(room.xPos * roomSize, 0f, room.yPos * roomSize);
        DungeonRoom newRoom = Instantiate(roomPrefabs[room.roomType], spawnPos, Quaternion.identity).GetComponent<DungeonRoom>();

        // Set doorways
        /*
         * 0 = up
         * 1 = down
         * 2 = left
         * 3 = right
         */
        for (int i = 0; i < 4; i++)
        {
            if (!room.adjRooms.ContainsKey(i))
            {
                newRoom.CreateBarrier(i, false);
                continue;
            }

            newRoom.GetDoorway(i).SetActive(true);
            newRoom.CreateBarrier(i, true);
        }
    }


}
