using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class ItemWeapon : ItemWithStats
{
    public float damage = 2f; // How much "damage" or effectiveness this tool has per hit

    [Header("Weapon Specifics")] public WeaponType weaponType; // The enum you defined earlier (Axe, Pickaxe, etc.)

    private void OnValidate()
    {
        itemType = ItemType.Weapon;
        maxStackSize = 1;
        slotTag = SlotTag.Hand;

        if (string.IsNullOrEmpty(title) || title == "New Item") title = "New Weapon";
        ValidateStats();
        CompileModifiers();
    }

    public override void ValidateStats()
    {
        statModifiers ??= new List<StatModifierEntry>();
        statModifiers.RemoveAll(mod => mod.statType == StatType.Attack);
        statModifiers.Add(new StatModifierEntry
        {
            statType = StatType.Attack,
            value = damage,
            type = StatModifierType.Replacement
        });
    }
}

public enum WeaponType
{
    Sword
}