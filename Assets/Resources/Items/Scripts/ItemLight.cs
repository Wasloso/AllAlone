using System;
using UnityEngine;



[CreateAssetMenu(fileName = "NewLight", menuName = "Items/Light Item", order = 4)]
public class ItemLight : ItemWithStats, ILight
{
    public Color lightColor = Color.white;
    public float intensity = 1f;
    public float range = 10f;

    public float Range => range;
    public Color LightColor => lightColor;
    public float Intensity => intensity;
    private void OnValidate()
    {
        itemType = ItemType.LightSource;
        maxStackSize = 1;
        slotTag = SlotTag.Hand;

        if (string.IsNullOrEmpty(title) || title == "New Item") title = "New Light Item";
    }
}
