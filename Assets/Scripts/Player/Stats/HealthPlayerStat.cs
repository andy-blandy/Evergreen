using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayerStat : PlayerUpgrade
{
    public override void SetBonus(float num)
    {
        Player.instance.playerHealth.health = Mathf.RoundToInt(num);
    }

    public override void AddToBonus(float num)
    {
        Player.instance.playerHealth.Heal(Mathf.RoundToInt(num));
    }

    public override void ResetStat()
    {
        Player.instance.playerHealth.health = 10;
    }
}
