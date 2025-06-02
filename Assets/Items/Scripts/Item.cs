using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Base Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("Basic Item Info")] public string id;

    public string title = "New Item";

    [TextArea(3, 5)] public string description = "A generic item.";

    public Sprite icon;
    public int maxStackSize = 16;


    [Header("Item Type")] public ItemType itemType = ItemType.Generic;
    [Header("Slot tag")] public SlotTag slotTag = SlotTag.None;
    [NonSerialized] public List<StatModifier> compiledModifiers = new();

    public bool Stackable => maxStackSize > 1;


    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id)) id = title + Guid.NewGuid();
    }
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

public enum SlotTag
{
    None,
    Head,
    Chest,
    Hand
}