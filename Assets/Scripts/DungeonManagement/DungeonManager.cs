using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //Variable declaration

    [Header("Positioning")]
    public Transform FirstDungeonRoom;
    [SerializeField] private float RoomDistanceApart = 45.0f; //Can now adjust the distance that the rooms spawn away from each other


    [Header("Room Counters")]
    [SerializeField] private int RoomsSpawned = 0;
    [SerializeField] private int ShopsSpawned = 0;

    [Header("Room Prefabs")]
    public GameObject DungeonBase;
    public GameObject[] spawnableRooms; //Array for the rooms that can be spawned into the dungeon
    public GameObject[] shopRooms;
    public GameObject bossRoom;

    [Header("Active Rooms")]
    public List<DungeonRoom> activeDungeonRooms = new List<DungeonRoom>();
    private DungeonRoom currentRoom;

    [Header("Prob of Rooms")]
    public int randomRoomChoice;
    public int randomShopChoice;
    public float BossRoomChance = 0.00f;
    private bool SpawnShop = false;

    // Room Location
    public Vector3 RoomLoc = Vector3.zero;
    public string exitDoorPosition = "";


    // Singleton
    public static DungeonManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void CheckDoorway(Collider P, string enterDoor)
    {
        /*
        * Get the position and doorway location of the next room
        */
        switch (enterDoor)
        {
            case "north":
                RoomLoc = new Vector3(currentRoom.transform.position.x, 0, currentRoom.transform.position.z + RoomDistanceApart);
                exitDoorPosition = "south";
                break;
            case "south":
                RoomLoc = new Vector3(currentRoom.transform.position.x, 0, currentRoom.transform.position.z - RoomDistanceApart);
                exitDoorPosition = "north";
                break;
            case "east":
                RoomLoc = new Vector3(currentRoom.transform.position.x + RoomDistanceApart, 0, currentRoom.transform.position.z);
                exitDoorPosition = "west";
                break;
            case "west":
                RoomLoc = new Vector3(currentRoom.transform.position.x - RoomDistanceApart, 0, currentRoom.transform.position.z);
                exitDoorPosition = "east";
                break;
        }

        if (!alreadyActiveRoom(P))
        {
            SelectRoom(P);
        }
    }

    //This will make new rooms depending on which door the player hit in the dungeon
    //For reference child 1 is rooms, child 0 is north, 1 is south, 2 is east, 3 is west
    public void SelectRoom(Collider P)
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

        if (ShopChance < .20f && ShopsSpawned < 2)
        {
            ShopsSpawned++;
            SpawnShop = true;
        }

        if (BossChance < BossRoomChance)
        {
            SpawnBossRoom(P);
            return;
        }
        else
        {
            SpawnRoom(P);
        }
    }

    private void SpawnRoom(Collider P)
    {

        /*
         * Spawn the room
         */
        GameObject newRoom;
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

        /*
         * Add the room to the activeDungeonRooms list
         */
        room = newRoom.GetComponent<DungeonRoom>();
        RoomsSpawned++;
        activeDungeonRooms.Add(room);


        /*
         * Move the player to the next room, and set the previous room to being inactive
         */
        GameObject door = room.GetDoorway(exitDoorPosition);
        door.SetActive(true);
        P.transform.position = door.transform.GetChild(0).position;
        currentRoom.gameObject.SetActive(false);
        currentRoom = room;
    }


    //checks to see if the room trying to be spawned is already spawned
    private bool alreadyActiveRoom(Collider P)
    {
        bool RoomSpawned = false;

        foreach(DungeonRoom i in activeDungeonRooms)
        {

            if(i.transform.position.Equals(RoomLoc))
            {
                GameObject prevRoom = currentRoom.gameObject;
                currentRoom = i;
                currentRoom.gameObject.SetActive(true);

                //Spawns player in the room of an already existing room when they try to traverse backwards
                P.transform.position = currentRoom.GetDoorway(exitDoorPosition).transform.position;

                RoomSpawned = true;

                prevRoom.SetActive(false);
            }
        }

        return RoomSpawned;
    } 

    //This is the first room that spawns in when the player enters the dungeon
    public void SpawnFirstRoom(Collider P)
    {
        GameObject NewRoom = Instantiate(DungeonBase, FirstDungeonRoom.position, Quaternion.identity);
        DungeonRoom newRoom = NewRoom.GetComponent<DungeonRoom>();
        activeDungeonRooms.Add(newRoom);
        currentRoom = newRoom;
        NewRoom.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.position;
    }

    private void SpawnBossRoom(Collider P)
    {
        Vector3 RoomLoc = new Vector3(0, 50, 500);
        GameObject NewRoom = Instantiate(bossRoom, RoomLoc, Quaternion.identity);
        DungeonRoom newRoom = NewRoom.GetComponent<DungeonRoom>();
        activeDungeonRooms.Add(newRoom);
        currentRoom = newRoom;
        P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.position;
        return;
    }
}
