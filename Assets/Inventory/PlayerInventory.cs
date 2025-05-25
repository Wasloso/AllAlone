using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int slotCount = 20;
    public List<InventorySlot> slots;

    private void Awake()
    {
        slots = new List<InventorySlot>(slotCount);
        for (var i = 0; i < slotCount; i++)
            slots.Add(new InventorySlot());
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        // Try stacking first
        if (item.maxStackSize > 1)
            foreach (var slot in slots)
            {
                var slotItem = slot.inventoryItem;
                if (slotItem.item == item && slotItem.quantity < item.maxStackSize)
                {
                    var addable = Mathf.Min(quantity, item.maxStackSize - slotItem.quantity);
                    slotItem.quantity += addable;
                    quantity -= addable;
                    if (quantity <= 0)
                        return true;
                }
            }

        // Add to empty slots
        // foreach (var slot in slots)
        //     if (slot.IsEmpty)
        //     {
        //         
        //         slot.inventoryItem = item;
        //         slot.quantity = quantity;
        //         return true;
        //     }

        return false; // Inventory full
    }
    //
    // public void RemoveItem(Item item, int quantity = 1)
    // {
    //     foreach (var slot in slots)
    //         if (slot.item == item)
    //         {
    //             slot.quantity -= quantity;
    //             if (slot.quantity <= 0)
    //             {
    //                 slot.item = null;
    //                 slot.quantity = 0;
    //             }
    //
    //             return;
    //         }
    // }
}