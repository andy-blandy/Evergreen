using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerStat : PlayerStat
{
    public override void SetStat(float num)
    {
        Player.instance.playerAttack.damage = Mathf.RoundToInt(num);
    }

    public override void Upgrade(float num)
    {
        Player.instance.playerAttack.damage += Mathf.RoundToInt(num);
    }
}
