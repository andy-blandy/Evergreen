using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public enum StatType
    {
        Health,
        Damage,
        Knockback
    }

    public Dictionary<StatType, PlayerStat> stats = new Dictionary<StatType, PlayerStat>();

    public static StatManager instance;
    void Awake()
    {
        instance = this;

        stats.Add(StatType.Health, new HealthPlayerStat());
        stats.Add(StatType.Damage, new DamagePlayerStat());
        stats.Add(StatType.Knockback, new KnockbackPlayerStat());
    }

    public void SetStat(StatType type, float num)
    {
        stats[type].SetStat(num);
    }

    public void UpgradeStat(StatType type, float upgradeAmount)
    {
        stats[type].Upgrade(upgradeAmount);
    }
}
