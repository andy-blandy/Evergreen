using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomGeneration))]
public class DungeonGeneration : MonoBehaviour
{
    public int minNumOfRooms = 7;
    public int maxNumOfRooms = 10;
    public int roomCount = 0;
    public float probOfGeneratingRoom = 0.5f;

    public Vector2Int dungeonSize;
    public Vector3 dungeonSpawnLocation;
    public Vector2 roomSize = new Vector2(60, 60);

    private Dictionary<Vector2Int, Room> roomGrid;
    private Queue<Room> unvisitedDungeonRooms;
    private List<Room> newDungeon;

    private RoomGeneration roomGeneration;

    void Awake()
    {
        roomGeneration = GetComponent<RoomGeneration>();
    }

    public List<Room> StartDungeonGeneration(bool spawnDungeon)
    {
        roomGrid = new Dictionary<Vector2Int, Room>();
        unvisitedDungeonRooms = new Queue<Room>();
        newDungeon = new List<Room>();

        // Create initial room
        Room firstRoom = new Room();
        roomGrid.Add(firstRoom.pos, firstRoom);
        roomCount++;
        unvisitedDungeonRooms.Enqueue(firstRoom);

        GenerateDungeon();

        if (spawnDungeon)
        {
            SpawnDungeon(newDungeon);
        }

        return newDungeon;
    }

    void GenerateDungeon()
    {
        while (unvisitedDungeonRooms.Count > 0)
        {
            GenerateAdjacentRooms(unvisitedDungeonRooms.Dequeue());
        }

        // Redo generation if not enough rooms created
        if (roomCount < minNumOfRooms)
        {
            foreach (Room room in newDungeon)
            {
                unvisitedDungeonRooms.Enqueue(room);
            }

            GenerateDungeon();
            return;
        }

        SetRoomTypes();
    }

    void GenerateAdjacentRooms(Room curRoom)
    {
        for (int dir = 0; dir < 4; dir++)
        {
            if (roomCount == maxNumOfRooms)
            {
                break;
            }

            if (curRoom.adjRooms.ContainsKey(dir))
            {
                continue;
            }

            float randomNum = UnityEngine.Random.Range(0f, 1f);

            if (randomNum < probOfGeneratingRoom)
            {
                AddRoom(dir, curRoom);
            }
        }

        if (!newDungeon.Contains(curRoom))
        {
            newDungeon.Add(curRoom);
        }
    }

    void AddRoom(int dir, Room prevRoom)
    {
        /*
         * 0 = up
         * 1 = down
         * 2 = left
         * 3 = right
         */
        int newXPos = prevRoom.xPos;
        int newYPos = prevRoom.yPos;
        int oppDir = -1;
        switch (dir) 
        {
            case 0:
                newYPos++;
                oppDir = 1;
                break;
            case 1:
                newYPos--;
                oppDir = 0;
                if (newYPos < 0)
                {
                    return;
                }
                break;
            case 2:
                newXPos--;
                oppDir = 3;
                break;
            case 3:
                newXPos++;
                oppDir = 2;
                break;
        }

        Vector2Int newPos = new Vector2Int(newXPos, newYPos);
        if (roomGrid.ContainsKey(newPos))
        {
            return;
        }

        Room newRoom = new Room(newXPos, newYPos, prevRoom.depth + 1);
        prevRoom.adjRooms.Add(dir, newRoom);
        newRoom.adjRooms.Add(oppDir, prevRoom);
        roomGrid.Add(newRoom.pos, newRoom);

        unvisitedDungeonRooms.Enqueue(newRoom);
        roomCount++;
    }

    void SetRoomTypes()
    {
        /*
         * 0 = Combat
         * 1 = Shop
         * 2 = Boss
         */

        int maxDepth = -1;
        Room deepestRoom = null;
        List<Room> shopChoices = new List<Room>();

        foreach (Room room in newDungeon)
        {
            room.roomType = 0;

            // Find the room furthest from the starting room to set as the boss room
            if (room.depth > maxDepth)
            {
                maxDepth = room.depth;
                deepestRoom = room;
            }

            if (room.depth >= 2)
            {
                shopChoices.Add(room);
            }
        }

        // Choose shop
        if (shopChoices.Contains(deepestRoom))
        {
            shopChoices.Remove(deepestRoom);
        }
        if (shopChoices.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, shopChoices.Count - 1);
            shopChoices[randomIndex].roomType = 1;
        }

        // Set boss room
        deepestRoom.roomType = 2;
    }

    public void SpawnDungeon(List<Room> dungeonRooms)
    {
        foreach (Room room in dungeonRooms)
        {
            roomGeneration.SpawnRoom(room, dungeonSpawnLocation);
            //room.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(dungeonSpawnLocation, 1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(dungeonSpawnLocation, new Vector3(roomSize.x, 0f, roomSize.y));
    }
}
