using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayerStat : PlayerUpgrade
{
    public override void SetBonus(float num)
    {
        Player.instance.playerAttack.knockbackAmount = num;
    }

    public override void AddToBonus(float num)
    {
        Player.instance.playerAttack.knockbackAmount += num;
    }
}

