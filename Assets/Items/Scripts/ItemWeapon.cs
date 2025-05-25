using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class ItemWeapon : Item // Renamed from ToolItemData to match pattern
{
    public float damage = 2f; // How much "damage" or effectiveness this tool has per hit

    // Add tool-specific properties (like ToolType, effectiveness)
    [Header("Weapon Specifics")] public WeaponType weaponType; // The enum you defined earlier (Axe, Pickaxe, etc.)

    private void OnValidate()
    {
        itemType = ItemType.Weapon;
        maxStackSize = 1;
        slotTag = SlotTag.Hand;

        if (string.IsNullOrEmpty(title) || title == "New Item") title = "New Weapon";
    }
}

public enum WeaponType
{
    Sword
}