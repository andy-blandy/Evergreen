using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //Variable declaration
    public GameObject DungeonBase;

    private List<Transform> ActiveDungeonRooms = new List<Transform>();
    public Transform FirstDungeonRoom;
    private GameObject CurrentRoom;
    private int RoomsSpawned;
    public float BossRoomChance = 0.00f;

    //Setting the first rooms transform as well as saving it to the active rooms
    private void Awake()
    {
        FirstDungeonRoom.transform.position = new Vector3(0, 0, 500);
        ActiveDungeonRooms.Add(FirstDungeonRoom);
    }

    //The next 4 functions are called when the player collides with the door, they are called from the Next Room script that is on all the doors
    public void NorthDoorHit(Collider P)
    {
        SpawnNewRoom(P, 1);
    }

    public void SouthDoorHit(Collider P)
    {
        SpawnNewRoom(P, 2);
    }

    public void EastDoorHit(Collider P)
    {
        SpawnNewRoom(P, 3);
    }

    public void WestDoorHit(Collider P)
    {
        SpawnNewRoom(P, 4);
    }

    //This will make new rooms depending on which door the player hit in the dungeon
    //For reference child 1 is rooms, child 0 is north, 1 is south, 2 is east, 3 is west
    private void SpawnNewRoom(Collider P, int x)
    {

        //Keep track of the number of rooms spawned as well as increment the counter for the chance at a boss room
        RoomsSpawned++;
        if (RoomsSpawned == 5 || RoomsSpawned == 10 || RoomsSpawned == 15 || RoomsSpawned == 20)
        {
            BossRoomChance += 0.10f;
        }
        else if(RoomsSpawned == 30)
        {
            BossRoomChance = 1;
        }

        float BossChance = Random.Range(0.00f, 1.01f);
        if (BossChance < BossRoomChance )
        {
            //Code in here for the boss room
            Debug.Log("Boss Room");
        }
        else //To spawn a random number of doors that the player has a possiblility of going through. not yet implemented
        {
            if (x == 1) // North
            {
                Vector3 RoomLoc = new Vector3(CurrentRoom.transform.position.x, 0, CurrentRoom.transform.position.z + 35);
                if (!alreadyActiveRoom(RoomLoc, P, x))
                {
                    GameObject NewRoom = Instantiate(DungeonBase, RoomLoc, Quaternion.identity);
                    ActiveDungeonRooms.Add(NewRoom.transform);
                    CurrentRoom = NewRoom;
                    P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.position;
                }
            }

            if (x == 2) // South
            {
                Vector3 RoomLoc = new Vector3(CurrentRoom.transform.position.x, 0, CurrentRoom.transform.position.z - 35);
                if (!alreadyActiveRoom(RoomLoc, P, x))
                {
                    GameObject NewRoom = Instantiate(DungeonBase, RoomLoc, Quaternion.identity);
                    ActiveDungeonRooms.Add(NewRoom.transform);
                    CurrentRoom = NewRoom;
                    P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.position;
                }
            }

            if (x == 3) // East
            {
                Vector3 RoomLoc = new Vector3(CurrentRoom.transform.position.x + 35, 0, CurrentRoom.transform.position.z);
                if (!alreadyActiveRoom(RoomLoc, P, x))
                {
                    GameObject NewRoom = Instantiate(DungeonBase, RoomLoc, Quaternion.identity);
                    ActiveDungeonRooms.Add(NewRoom.transform);
                    CurrentRoom = NewRoom;
                    P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(3).transform.GetChild(0).transform.position;
                }
            }

            if (x == 4) // West
            {
                Vector3 RoomLoc = new Vector3(CurrentRoom.transform.position.x - 35, 0, CurrentRoom.transform.position.z);
                if (!alreadyActiveRoom(RoomLoc, P, x))
                {
                    GameObject NewRoom = Instantiate(DungeonBase, RoomLoc, Quaternion.identity);
                    ActiveDungeonRooms.Add(NewRoom.transform);
                    CurrentRoom = NewRoom;
                    P.transform.position = NewRoom.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).transform.position;
                }
            }
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
                    P.transform.position = CurrentRoom.transform.GetChild(1).transform.GetChild(3).transform.GetChild(0).transform.position;
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
}
