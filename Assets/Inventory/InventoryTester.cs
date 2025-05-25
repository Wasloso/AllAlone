using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    // public PlayerInventory playerInventory;
    public Item testItem;
    public int testAmount = 5;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
        } // Press I to add item
        // if (playerInventory != null && testItem != null)
        // {
        //     var added = playerInventory.AddItem(testItem, testAmount);
        //     Debug.Log($"Tried to add {testAmount}x {testItem.itemName}. Actually added: {added}");
        // }
    }
}