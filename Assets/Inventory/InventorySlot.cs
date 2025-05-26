using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItem inventoryItem;
    public SlotTag slotTag = SlotTag.None;
    public bool IsEmpty => inventoryItem == null || inventoryItem.count == 0;


    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (dropped == null) return;
        if (slotTag != SlotTag.None && dropped.item.slotTag != slotTag)
        {
            Debug.Log($"Wrong slot! Item slot: {dropped.item.slotTag}, slot tag {slotTag} ");
            return;
        }

        dropped.parentAfterDrag = transform;
        inventoryItem = dropped;
    }
}