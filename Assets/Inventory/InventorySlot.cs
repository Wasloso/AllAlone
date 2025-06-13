using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public SlotTag slotTag = SlotTag.None;
    public InventoryItem _inventoryItem { get; private set; }
    public bool IsEmpty => inventoryItem == null || inventoryItem.count == 0;

    public InventoryItem inventoryItem
    {
        get => _inventoryItem;
        set
        {
            _inventoryItem = value;
            if (_inventoryItem)
            {
                _inventoryItem.transform.SetParent(transform);
                _inventoryItem.transform.localPosition = Vector3.zero;
            }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var interactingPlayer = GetPlayerFromPointerEventData(eventData);
            if (interactingPlayer == null || inventoryItem.item == null) return;
            if (inventoryItem.item is ItemConsumable consumable)
            {
                interactingPlayer.Consume(consumable);
                inventoryItem.count--;

                if (inventoryItem.count <= 0)
                {
                    Destroy(inventoryItem.gameObject);
                    inventoryItem = null;
                }
                else
                {
                    inventoryItem.RefreshCount();
                }

                OnItemChanged?.Invoke();
            }
        }
    }

    public event Action OnItemChanged;

    private IConsumableReciever GetPlayerFromPointerEventData(PointerEventData eventData)
    {
        // not sure how to do this right - it doesnt work
        if (eventData.currentInputModule is InputSystemUIInputModule inputSystemUI)
        {
            Debug.Log("Found input");
            // The PlayerInput instance that is currently controlling this UI input module.
            var playerInput = inputSystemUI.GetComponent<PlayerInput>();
            if (playerInput != null)
                // Assuming your Player.Player script is on the same GameObject as PlayerInput,
                // or a child/parent that can be found.
                return playerInput.GetComponent<Player.Player>();
            // If Player.Player is on a separate GameObject that's not parent/child of PlayerInput:
            // You'd need a PlayerManager that maps PlayerInput.playerIndex to Player.Player instance.
            // Example: return PlayerManager.Instance.GetPlayerByPlayerInput(playerInput);
        }

        // Fallback for debugging or simpler setups, but unreliable for proper multiplayer.
        // It will just find the first Player.Player component in the scene.
        Debug.LogWarning(
            "GetPlayerFromPointerEventData: Could not determine player from UI Input System. Falling back to FindObjectOfType<Player.Player>(). For multiplayer, ensure PlayerInput and InputSystemUIInputModule are properly linked.");
        return FindObjectOfType<Player.Player>();
    }
}