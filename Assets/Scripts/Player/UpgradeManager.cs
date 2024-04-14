using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public enum UpgradeType
    {
        Health,
        Damage,
        Knockback
    }

    public Dictionary<UpgradeType, PlayerUpgrade> stats = new Dictionary<UpgradeType, PlayerUpgrade>();

    public static UpgradeManager instance;
    void Awake()
    {
        instance = this;

        stats.Add(UpgradeType.Health, new HealthPlayerStat());
        stats.Add(UpgradeType.Damage, new DamagePlayerStat());
        stats.Add(UpgradeType.Knockback, new KnockbackPlayerStat());
    }

    public void SetBonus(UpgradeType type, float num)
    {
        stats[type].SetBonus(num);
    }

    public void AddToBonus(UpgradeType type, float upgradeAmount)
    {
        stats[type].AddToBonus(upgradeAmount);
    }

    public void ResetUpgrades()
    {
        foreach (PlayerUpgrade upgrade in stats.Values)
        {
            upgrade.SetBonus(0);
        }
    }
}
