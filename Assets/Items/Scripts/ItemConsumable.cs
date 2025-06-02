using System;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "NewConsumable", menuName = "Items/Consumable Item", order = 1)]
    public class ItemConsumable : Item
    {
        [Header("Consumable Settings")] public float restoreHealth;

        public float restoreHunger;
        public float restoreSanity;
        public bool isOneTimeUse = true;

        private void OnValidate()
        {
            itemType = ItemType.Consumable;

            slotTag = SlotTag.None;

            if (string.IsNullOrEmpty(title) || title == "New Item") title = "New Consumable";
            if (string.IsNullOrEmpty(id)) id = title + Guid.NewGuid();
        }
    }
}