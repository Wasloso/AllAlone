using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItem inventoryItem;
    public bool IsEmpty => inventoryItem == null || inventoryItem.quantity == 0;


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
        dropped.parentAfterDrag = transform;
        inventoryItem = dropped;
    }
}