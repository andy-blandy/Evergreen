using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int xPos;
    public int yPos;
    public int depth;

    public int roomType;

     /*
     * 0 = up
     * 1 = down
     * 2 = left
     * 3 = right
     */
    public Dictionary<int, Room> adjRooms;

    public Room()
    {
        xPos = 0; yPos = 0;
        depth = 0;

        adjRooms = new Dictionary<int, Room>();
    }

    public Room(int x, int y)
    {
        xPos = x;
        yPos = y;

        adjRooms = new Dictionary<int, Room>();
    }

    public Room(int x, int y, int depth)
    {
        xPos = x;
        yPos = y;
        this.depth = depth;

        adjRooms = new Dictionary<int, Room>();
    }
}
