using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBossRoom : DungeonRoom
{
    void Awake()
    {
        LockRoom(true);
    }

    public override void EnterRoom()
    {
        LockRoom(true);
    }
}
