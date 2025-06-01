using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public InventorySlot handSlot;
    public InventorySlot headSlot;
    public InventorySlot chestSlot;

    public bool AddItem(Item item, int quantity = 1)
    {
        for (var i = 0; i < inventorySlots.Length; i++)
        {
            var slot = inventorySlots[i];
            var itemInSlot = slot.inventoryItem;
            if (!itemInSlot || !itemInSlot.item) continue;
            if (itemInSlot.item.id != item.id)
            {
                Debug.Log(itemInSlot.item.id + " doesn't equal " + item.id);
                continue;
            }

            if (itemInSlot.count < itemInSlot.item.maxStackSize)
            {
                var maxQuantityToAdd = Math.Min(itemInSlot.item.maxStackSize - itemInSlot.count, quantity);
                itemInSlot.count += maxQuantityToAdd;
                quantity -= maxQuantityToAdd;
                itemInSlot.RefreshCount();
                if (quantity <= 0) return true;
            }
        }

        Debug.Log("No mathes found for: " + item.name);

        for (var i = 0; i < inventorySlots.Length; i++)
        {
            var slot = inventorySlots[i];
            var itemInSlot = slot.inventoryItem;
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }
    public int CountItem(Item item)
    {
        int total = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot.inventoryItem != null && slot.inventoryItem.item == item)
            {
                total += slot.inventoryItem.count;
            }
        }
        return total;
    }

    public void RemoveItem(Item item, int quantity)
    {
        foreach (var slot in inventorySlots)
        {
            var i = slot.inventoryItem;
            if (i != null && i.item == item)
            {
                int toRemove = Mathf.Min(quantity, i.count);
                i.count -= toRemove;
                quantity -= toRemove;

                if (i.count == 0)
                {
                    Destroy(i.gameObject);
                    slot.inventoryItem = null;
                }
                else
                {
                    i.RefreshCount();
                }

                if (quantity <= 0) return;
            }
        }
    }



    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        var newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        var inventoryItem = newItemGo.GetComponent<InventoryItem>();
        slot.inventoryItem = inventoryItem;
        inventoryItem.InitializeItem(item);
    }
}