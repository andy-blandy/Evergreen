using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int pos;
    public int xPos;
    public int yPos;
    public int depth;

    /*
     * 0 = Combat
     * 1 = Shop
     * 2 = Boss
     */
    public int roomType;

    public DungeonRoom dungeonRoom;
    public GameObject gameObject;

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
        pos = Vector2Int.zero;
        depth = 0;

        adjRooms = new Dictionary<int, Room>();
    }

    public Room(int x, int y)
    {
        xPos = x;
        yPos = y;
        pos = new Vector2Int(x, y);

        adjRooms = new Dictionary<int, Room>();
    }

    public Room(int x, int y, int depth)
    {
        xPos = x;
        yPos = y;
        this.depth = depth;
        pos = new Vector2Int(x, y);

        adjRooms = new Dictionary<int, Room>();
    }
}
