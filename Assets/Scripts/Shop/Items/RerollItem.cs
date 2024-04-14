using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollItem : ShopItem
{
    public override void Use()
    {
        Shop.instance.Reroll();
    }
}
