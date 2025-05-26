using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image image;
    public TMP_Text countText;
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
        image.raycastTarget = true;
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        var textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void InitializeItem(Item newItem, int quantity = 1)
    {
        item = newItem;
        image.sprite = newItem.icon;
        count = quantity;
        RefreshCount();
    }
}