using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DungeonGeneration))]
public class NewDungeonManager : MonoBehaviour
{
    // Component References
    private DungeonGeneration dungeonGeneration;

    // Room Camera
    public bool isRoomCameraEnabled;

    public List<Room> dungeon;

    public static NewDungeonManager instance;
    private void Awake()
    {
        instance = this;
        dungeonGeneration = GetComponent<DungeonGeneration>();
    }

    void Start()
    {
        dungeon = dungeonGeneration.StartDungeonGeneration(true);

        isRoomCameraEnabled = true;
    }

    public void GenerateNewDungeon()
    {
        foreach (Room room in dungeon)
        {
            Destroy(room.gameObject);
        }

        dungeon = dungeonGeneration.StartDungeonGeneration(true);
    }

}
