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
                    dungeonManager.NorthDoorHit(collision);
                    break;
                case "SouthDoor":
                    dungeonManager.SouthDoorHit(collision);
                    break;
                case "EastDoor":
                    dungeonManager.EastDoorHit(collision);
                    break;
                case "WestDoor":
                    dungeonManager.WestDoorHit(collision);
                    break;
                case "DungeonStart":
                    dungeonManager.SpawnFirstRoom(collision);
                    break;
            }
        }
    }
}
