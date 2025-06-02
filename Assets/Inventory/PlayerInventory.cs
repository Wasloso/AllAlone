using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public InventorySlot handSlot;
    public InventorySlot headSlot;
    public InventorySlot chestSlot;


    private void Awake()
    {
        if (handSlot != null) handSlot.OnItemChanged += HandleHandSlotItemChanged;
        if (headSlot != null) headSlot.OnItemChanged += HandleHeadSlotChanged;
        if (chestSlot != null) chestSlot.OnItemChanged += HandleChestSlotItemChanged;
    }

    public event Action<SlotTag, Item> OnHandSlotChanged;
    public event Action<SlotTag, Item> OnChestSlotChanged;
    public event Action<SlotTag, Item> OnHeadSlotChanged;


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
        var total = 0;
        foreach (var slot in inventorySlots)
            if (slot.inventoryItem != null && slot.inventoryItem.item == item)
                total += slot.inventoryItem.count;

        return total;
    }

    public void RemoveItem(Item item, int quantity)
    {
        foreach (var slot in inventorySlots)
        {
            var i = slot.inventoryItem;
            if (i != null && i.item == item)
            {
                var toRemove = Mathf.Min(quantity, i.count);
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

    private void HandleHandSlotItemChanged()
    {
        var currentEquippedItem = GetEquippedItem(SlotTag.Hand);
        OnHandSlotChanged?.Invoke(SlotTag.Hand, currentEquippedItem);
    }

    private void HandleChestSlotItemChanged()
    {
        var currentEquippedItem = GetEquippedItem(SlotTag.Chest);
        OnChestSlotChanged?.Invoke(SlotTag.Chest, currentEquippedItem);
    }

    private void HandleHeadSlotChanged()
    {
        var currentEquippedItem = GetEquippedItem(SlotTag.Head);
        OnHeadSlotChanged?.Invoke(SlotTag.Head, currentEquippedItem);
    }


    public Item GetEquippedItem(SlotTag slotTag)
    {
        switch (slotTag)
        {
            case SlotTag.Head:
                if (headSlot != null && headSlot.inventoryItem != null) return headSlot.inventoryItem.item;
                return null;
            case SlotTag.Chest:
                if (chestSlot != null && chestSlot.inventoryItem != null) return chestSlot.inventoryItem.item;
                return null;
            case SlotTag.Hand:
                if (handSlot != null && handSlot.inventoryItem != null) return handSlot.inventoryItem.item;
                return null;
        }

        return null;
    }
}