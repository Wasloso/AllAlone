using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public SlotTag slotTag = SlotTag.None;
    private InventoryItem _inventoryItem;
    public bool IsEmpty => inventoryItem == null || inventoryItem.count == 0;

    public InventoryItem inventoryItem
    {
        get => _inventoryItem;
        set
        {
            _inventoryItem = value;
            OnItemChanged?.Invoke();
        }
    }

    private void Start()
    {
    }


    private void Update()
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        var droppedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (droppedItem == null) return;


        if (slotTag != SlotTag.None && droppedItem.item.slotTag != slotTag)
        {
            Debug.Log(
                $"❌ Can't place {droppedItem.item.title} in {slotTag} slot. Requires: {droppedItem.item.slotTag}");
            return;
        }

        var fromSlot = droppedItem.parentAfterDrag?.GetComponent<InventorySlot>();
        var targetItem = inventoryItem;


        if (targetItem == null)
        {
            droppedItem.parentAfterDrag = transform;
            inventoryItem = droppedItem;

            if (fromSlot != null)
                fromSlot.inventoryItem = null;

            return;
        }


        if (targetItem.item.id == droppedItem.item.id && targetItem.count < targetItem.item.maxStackSize)
        {
            var total = targetItem.count + droppedItem.count;
            var max = targetItem.item.maxStackSize;

            var overflow = Mathf.Max(0, total - max);
            targetItem.count = Mathf.Min(total, max);
            targetItem.RefreshCount();

            if (overflow == 0)
            {
                Destroy(droppedItem.gameObject);
                if (fromSlot != null)
                    fromSlot.inventoryItem = null;
            }
            else
            {
                droppedItem.count = overflow;
                droppedItem.RefreshCount();
            }

            return;
        }


        if (fromSlot != null && fromSlot.slotTag != SlotTag.None && targetItem.item.slotTag != fromSlot.slotTag)
        {
            Debug.Log(
                $"❌ Can't move {targetItem.item.title} to {fromSlot.slotTag} slot. Requires: {targetItem.item.slotTag}");
            return;
        }


        droppedItem.parentAfterDrag = transform;
        targetItem.transform.SetParent(fromSlot.transform);
        targetItem.transform.localPosition = Vector3.zero;


        inventoryItem = droppedItem;
        fromSlot.inventoryItem = targetItem;
    }

    public event Action OnItemChanged;
}