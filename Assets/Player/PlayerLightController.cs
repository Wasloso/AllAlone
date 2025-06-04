using UnityEngine;
using Items;

public class PlayerLightController : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Light heldLight;

    private void OnEnable()
    {
        inventory.OnHandSlotChanged += HandleHandSlotChanged;
    }

    private void OnDisable()
    {
        inventory.OnHandSlotChanged -= HandleHandSlotChanged;
    }

    private void Start()
    {
        HandleHandSlotChanged(SlotTag.Hand, inventory.GetEquippedItem(SlotTag.Hand));
    }

    private void HandleHandSlotChanged(SlotTag slot, Item item)
    {
        if (slot != SlotTag.Hand) return;

        if (item is ILight lightItem)
        {
            heldLight.enabled = true;
            heldLight.color = lightItem.LightColor;
            heldLight.intensity = lightItem.Intensity;
            heldLight.range = lightItem.Range;
        }
        else
        {
            heldLight.enabled = false;
        }
    }
}
