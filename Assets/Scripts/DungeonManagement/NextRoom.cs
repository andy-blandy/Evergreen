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
                    dungeonManager.SelectRoom(collision, 1);
                    break;
                case "SouthDoor":
                    dungeonManager.SelectRoom(collision, 2);
                    break;
                case "EastDoor":
                    dungeonManager.SelectRoom(collision, 3);
                    break;
                case "WestDoor":
                    dungeonManager.SelectRoom(collision, 4);
                    break;
                case "DungeonStart":
                    dungeonManager.SpawnFirstRoom(collision);
                    break;
                default:
                    dungeonManager.SelectRoom(collision, 1);
                    break;
            }
        }
    }
}
