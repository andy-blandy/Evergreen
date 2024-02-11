using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayerStat : PlayerStat
{
    public override void SetStat(float num)
    {
        Player.instance.playerAttack.knockbackAmount = num;
    }

    public override void Upgrade(float num)
    {
        Player.instance.playerAttack.knockbackAmount += num;
    }
}

