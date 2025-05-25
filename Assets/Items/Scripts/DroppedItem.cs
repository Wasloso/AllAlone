using UnityEngine;

public class DroppedItem : InteractableObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AutoBoxCollider autoBoxCollider;
    private Item item;
    private int quantity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void Initialize(Item item, int quantity = 0)
    {
        this.item = item;
        this.quantity = quantity;
        spriteRenderer.sprite = item.icon;
        if (autoBoxCollider)
            autoBoxCollider.Refresh();
    }

    public override void Interact(GameObject interactor, ItemTool toolUsed = null)
    {
        Debug.Log($"DroppedItem {gameObject.name}, item: {item.name}, quantity: {quantity}");
    }
}