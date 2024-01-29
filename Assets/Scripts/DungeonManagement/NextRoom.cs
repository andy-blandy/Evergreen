using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoom : MonoBehaviour
{
    private GameObject DungeonManager;

    //To find the dungeon manager Script
    private void Awake()
    {
        DungeonManager = GameObject.FindGameObjectWithTag("DungeonManager");
    }

    //This determines what function to call in the Dungeon manager Script 
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Playr" )
        {
            string s = gameObject.tag;
            switch (s)
            {
                case "NorthDoor":
                    DungeonManager.GetComponent<DungeonManager>().NorthDoorHit(collision);
                    break;
                case "SouthDoor":
                    DungeonManager.GetComponent<DungeonManager>().SouthDoorHit(collision);
                    break;
                case "EastDoor":
                    DungeonManager.GetComponent<DungeonManager>().EastDoorHit(collision);
                    break;
                case "WestDoor":
                    DungeonManager.GetComponent<DungeonManager>().WestDoorHit(collision);
                    break;
                case "DungeonStart":
                    DungeonManager.GetComponent<DungeonManager>().SpawnFirstRoom(collision);
                    break;
            }
        }
    }
}
