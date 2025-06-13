using UnityEngine;

[CreateAssetMenu(fileName = "NewTool", menuName = "Items/Tool", order = 3)]
public class ItemTool : ItemWithStats
{
    public float effectiveness = 1f;

    [Header("Tool Specifics")] public ToolType toolType;

    private void OnValidate()
    {
        itemType = ItemType.Tool;
        maxStackSize = 1;
        slotTag = SlotTag.Hand;

        if (string.IsNullOrEmpty(title) || title == "New Item") title = "New Tool";
    }
}

public enum ToolType
{
    Axe,
    Pickaxe,
    Shovel,
    Other,
    None
}