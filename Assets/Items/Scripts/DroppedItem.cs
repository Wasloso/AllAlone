using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AutoBoxCollider autoBoxCollider;
    private Item item;
    private int quantity;

    public bool CanInteract(GameObject interactor)
    {
        return true;
    }

    public void Interact(GameObject interactor, ItemTool toolUsed = null)
    {
        Debug.Log("Interact with dropped item");
        var playerInventory = interactor.GetComponent<PlayerInventory>();
        if (playerInventory == null) return;
        Debug.Log("Found player inventory");

        if (playerInventory.AddItem(item, quantity)) Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void Initialize(Item item, int quantity = 0)
    {
        this.item = item;
        this.quantity = quantity;
        spriteRenderer.sprite = item.icon;
        if (autoBoxCollider)
            autoBoxCollider.Refresh();
    }
}