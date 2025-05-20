using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Base Item", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Basic Item Info")] public string id = Guid.NewGuid().ToString();

    public string itemName = "New Item";

    [TextArea(3, 5)] public string description = "A generic item.";

    public Sprite icon;
    public int maxStackSize = 16;

    [Header("Modifiers")] public List<StatModifierEntry> statModifiers;

    [Header("Item Type")] public ItemType itemType = ItemType.Generic;
}

public enum ItemType
{
    Generic,
    Weapon,
    Armor,
    Tool,
    Consumable,
    Resource,
    Artifact
}