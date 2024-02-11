using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ShopItem", order = 0)]
public class ShopItem : ScriptableObject
{
    public StatManager.StatType stat;
    public float statChange;
    public int cost;
    [TextArea(2,10)] public string description;
}
