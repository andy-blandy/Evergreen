using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public int minNumOfRooms = 7;
    public int maxNumOfRooms = 10;
    public int roomCount = 0;
    public float probOfGeneratingRoom = 0.5f;

    private Queue<Room> unvisitedDungeonRooms;
    private List<Room> dungeonRooms;

    void Start()
    {
        StartDungeonGeneration();
        RoomGeneration.instance.SpawnDungeon(dungeonRooms);
    }



    public List<Room> StartDungeonGeneration()
    {
        unvisitedDungeonRooms = new Queue<Room>();
        dungeonRooms = new List<Room>();

        // Create initial room
        Room firstRoom = new Room();
        roomCount++;
        unvisitedDungeonRooms.Enqueue(firstRoom);

        GenerateDungeon();

        return dungeonRooms;
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
            foreach (Room room in dungeonRooms)
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

        if (!dungeonRooms.Contains(curRoom))
        {
            dungeonRooms.Add(curRoom);
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

        Room newRoom = new Room(newXPos, newYPos, prevRoom.depth + 1);
        prevRoom.adjRooms.Add(dir, newRoom);
        newRoom.adjRooms.Add(oppDir, prevRoom);

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

        foreach (Room room in dungeonRooms)
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
        int randomIndex = UnityEngine.Random.Range(0, shopChoices.Count - 1);
        shopChoices[randomIndex].roomType = 1;

        // Set boss room
        deepestRoom.roomType = 2;
    }
}
