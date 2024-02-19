using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoom : MonoBehaviour
{
    private DungeonManager dungeonManager;

    //To find the dungeon manager Script
    private void Start()
    {
        dungeonManager = DungeonManager.instance;
    }

    //This determines what function to call in the Dungeon manager Script 
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            string s = gameObject.tag;
            switch (s)
            {
                case "NorthDoor":
                    dungeonManager.CheckDoorway(collision, "north");
                    break;
                case "SouthDoor":
                    dungeonManager.CheckDoorway(collision, "south");
                    break;
                case "EastDoor":
                    dungeonManager.CheckDoorway(collision, "east");
                    break;
                case "WestDoor":
                    dungeonManager.CheckDoorway(collision, "west");
                    break;
                case "DungeonStart":
                    dungeonManager.SpawnFirstRoom(collision);
                    break;
                default:
                    dungeonManager.CheckDoorway(collision, "");
                    break;
            }
        }
    }
}
