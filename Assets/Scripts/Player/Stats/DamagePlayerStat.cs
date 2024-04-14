using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerStat : PlayerUpgrade
{
    public override void SetBonus(float num)
    {
        Player.instance.playerAttack.damage = Mathf.RoundToInt(num);
    }

    public override void AddToBonus(float num)
    {
        Player.instance.playerAttack.damage += Mathf.RoundToInt(num);
    }
}
