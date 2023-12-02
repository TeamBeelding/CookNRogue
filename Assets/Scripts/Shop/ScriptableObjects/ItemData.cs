using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "ItemData/New Item")]
public class ItemData : ScriptableObject
{
    public string name;
    public string description;
    public int cost;
    public Sprite inventorySprite;
}
