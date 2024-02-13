using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //Variable declaration
    public GameObject DungeonBase;

    public Transform FirstDungeonRoom;
    private GameObject CurrentRoom;
    private int RoomsSpawned;
    private float RoomDistanceApart = 45.0f; //Can now adjust the distance that the rooms spawn away from each other
    private int ShopRoomSpawnded = 0;
    private bool SpawnShop = false;

    [Header("Rooms")]
    public List<DungeonRoom> activeDungeonRooms = new List<DungeonRoom>();
    public GameObject[] spawnableRooms; //Array for the rooms that can be spawned into the dungeon
    public GameObject[] shopRooms;
    public GameObject bossRoom;

    [Header("Prob of Rooms")]
    public int randomRoomChoice;
    public int randomShopChoice;
    public float BossRoomChance = 0.00f;


    // Singleton
    public static DungeonManager instance;

    private void Awake()
    {
        instance = this;
    }

    //The next 4 functions are called when the player collides with the door, they are called from the Next Room script that is on all the doors
    public void NorthDoorHit(Collider P)
    {
        SelectRoom(P, 1);
    }

    public void SouthDoorHit(Collider P)
    {
        SelectRoom(P, 2);
    }

    public void EastDoorHit(Collider P)
    {
        SelectRoom(P, 3);
    }

    public void WestDoorHit(Collider P)
    {
        SelectRoom(P, 4);
    }

    //This will make new rooms depending on which door the player hit in the dungeon
    //For reference child 1 is rooms, child 0 is north, 1 is south, 2 is east, 3 is west
    private void SelectRoom(Collider P, int x)
    {
        randomRoomChoice = Random.Range(0, spawnableRooms.Length);
        randomShopChoice = Random.Range(0, shopRooms.Length);

        //Keep track of the number of rooms spawned as well as increment the counter for the chance at a boss room
        if (RoomsSpawned == 5 || RoomsSpawned == 10 || RoomsSpawned == 15 || RoomsSpawned == 20)
        {
            BossRoomChance += 0.10f;
        }
        else if(RoomsSpawned == spawnableRooms.Length)
        {
            BossRoomChance = 1;
        }

        //random values chance for different room spawns like the boss room and shop rooms
        float BossChance = Random.Range(0.00f, 1.01f);
        float ShopChance = Random.Range(0.00f, 1.01f);

        if (ShopChance < .20f && ShopRoomSpawnded < 2)
        {
            ShopRoomSpawnded++;
            SpawnShop = true;
        }

        if (BossChance < BossRoomChance)
        {
            SpawnBossRoom(P);
            return;
        }
        else
        {
            SpawnRoom(P, x);
        }
    }

    private void SpawnRoom(Collider P, int x)
    {
        GameObject newRoom;

        /*
         * Get the position and doorway location of the room to spawn
         */
        Vector3 RoomLoc = Vector3.zero;
        string exitDoorPosition = "";
        switch (x)
        {
            case 1:
                RoomLoc = new Vector3(CurrentRoom.transform.position.x, 0, CurrentRoom.transform.position.z + RoomDistanceApart);
                exitDoorPosition = "north";
                break;
            case 2:
                RoomLoc = new Vector3(CurrentRoom.transform.position.x, 0, CurrentRoom.transform.position.z - RoomDistanceApart);
                exitDoorPosition = "south";
                break;
            case 3:
                RoomLoc = new Vector3(CurrentRoom.transform.position.x + RoomDistanceApart, 0, CurrentRoom.transform.position.z);
                exitDoorPosition = "east";
                break;
            case 4:
                RoomLoc = new Vector3(CurrentRoom.transform.position.x - RoomDistanceApart, 0, CurrentRoom.transform.position.z);
                exitDoorPosition = "west";
                break;
        }

        if (!alreadyActiveRoom(RoomLoc, P, x))
        {
            DungeonRoom room;

            if (SpawnShop == true)
            {
                newRoom = Instantiate(shopRooms[randomShopChoice], RoomLoc, Quaternion.identity);
                SpawnShop = false;
            }
            else
            {
                newRoom = Instantiate(spawnableRooms[randomRoomChoice], RoomLoc, Quaternion.identity);
            }

            room = newRoom.GetComponent<DungeonRoom>();
            GameObject door = room.GetDoorway(exitDoorPosition);
            door.SetActive(true);

            RoomsSpawned++;
            activeDungeonRooms.Add(room);
            CurrentRoom = newRoom;
            P.transform.position = door.transform.GetChild(0).position;
            return;
        }
    }
    

    //checks to see if the room trying to be spawned is already spawned
    private bool alreadyActiveRoom(Vector3 t, Collider P, int x)
    {
        bool RoomSpawned = false;

        foreach(Transform i in ActiveDungeonRooms)
        {
            if(i.transform.position == t)
            {
                //Spawns player in the room of an already existing room when they try to traverse backwards
                if (x == 1) //north
                {
                    CurrentRoom = i.gameObject;
                    P.transform.position = CurrentRoom.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.position;
                }

                if (x == 2) //south
                {
                    CurrentRoom = i.gameObject;
                    P.transform.position = CurrentRoom.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.position;
                }

                if (x == 3) //east
                {
                    CurrentRoom = i.gameObject;
                    P.transform.position = CurrentRoom.transform.GetChild(1).GetChild(3).transform.GetChild(0).transform.position;
                }

                if (x == 4) //west
                {
                    CurrentRoom = i.gameObject;
                    P.transform.position = CurrentRoom.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).transform.position;
                }

                RoomSpawned = true;
            }
        }

        return RoomSpawned;
    } 

    //This is the first room that spawns in when the player enters the dungeon
    public void SpawnFirstRoom(Collider P)
    {
        GameObject NewRoom = Instantiate(DungeonBase, FirstDungeonRoom.position, Quaternion.identity);
        ActiveDungeonRooms.Add(NewRoom.transform);
        CurrentRoom = NewRoom;
        NewRoom.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.position;
    }

    private void SpawnBossRoom(Collider P)
    {
        Vector3 RoomLoc = new Vector3(0, 50, 500);
        GameObject NewRoom = Instantiate(spawnableRooms[0], RoomLoc, Quaternion.identity);
        ActiveDungeonRooms.Add(NewRoom.transform);
        CurrentRoom = NewRoom;
        P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.position;
        return;
    }
}
