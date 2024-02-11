using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayerStat : PlayerStat
{
    public override void SetStat(float num)
    {
        Player.instance.playerHealth.health = Mathf.RoundToInt(num);
    }

    public override void Upgrade(float num)
    {
        Player.instance.playerHealth.Heal(Mathf.RoundToInt(num));
    }
}
