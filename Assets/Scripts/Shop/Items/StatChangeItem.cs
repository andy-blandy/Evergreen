using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeItem : ShopItem
{
    public float boostAmount;
    public UpgradeManager.UpgradeType upgradeType;

    public override void Use()
    {
        UpgradeManager.instance.AddToBonus(upgradeType, boostAmount);
    }
}
