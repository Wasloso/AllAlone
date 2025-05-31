using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image image;
    public TMP_Text countText;
    public GameObject droppedItemPrefab;
    [HideInInspector] public Transform parentAfterDrag;
    public int count = 1;

    private void Start()
    {
        InitializeItem(item);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;

        var oldSlot = parentAfterDrag.GetComponent<InventorySlot>();
        if (oldSlot && oldSlot.inventoryItem == this)
            oldSlot.inventoryItem = null;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;

        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        InventorySlot slot = null;

        foreach (var resultItem in result)
        {
            slot = resultItem.gameObject.GetComponent<InventorySlot>();
            if (slot != null) break;
        }
        if (slot != null)
        {
            transform.SetParent(slot.transform);
            transform.localPosition = Vector3.zero;
            slot.inventoryItem = this;
            parentAfterDrag = slot.transform;
        }
        else
            Drop(eventData.position);

    }

    public void Drop(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPos = hit.point;

            for (int i = 0; i < count; i++)
            {
                worldPos.x += 0.1f * i; 
                worldPos.z += 0.1f * i; 
                var drop = Instantiate(droppedItemPrefab, worldPos, Quaternion.identity);
                var dropped = drop.GetComponent<DroppedItem>();
                if (dropped != null)
                {
                    dropped.Initialize(item, 1);
                    Debug.Log($"Dropped {item.name} at {worldPos}");
                }

                Destroy(gameObject);
            }
        }
    }


    public void RefreshCount()
    {
        countText.text = count.ToString();
        var textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void InitializeItem(Item newItem, int quantity = 1)
    {
        Debug.Log("Initializing item: " + newItem.name);
        item = newItem;
        image.sprite = newItem.icon;
        count = quantity;
        RefreshCount();
    }
}