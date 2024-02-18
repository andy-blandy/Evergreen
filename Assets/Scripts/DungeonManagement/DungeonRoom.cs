using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public GameObject northDoorway, southDoorway, westDoorway, eastDoorway;

    /// <summary>
    /// Returns the doorway corresponding to the direction given in string form.
    /// </summary>
    /// <param name="direction">"north", "south", "east", "west"</param>
    /// <returns>gameObject or null</returns>
    public GameObject GetDoorway(string direction)
    {
        switch (direction)
        {
            case "north":
                return northDoorway;
            case "south":
                return southDoorway;
            case "east":
                return westDoorway;
            case "west":
                return eastDoorway;
            default:
                Debug.LogAssertion(direction + " is not a valid direction");
                return null;
        }
    }
}
