using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager: MonoBehaviour
{
    public GameObject player;
    public List<Reciepie> reciepes;
    private PlayerInventory playerInventory;


    public void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory component not found on player object.");
        }
    }
    public bool CanCraft(Reciepie recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            int owned = playerInventory.CountItem(ingredient.item);
            if (owned < ingredient.quantity)
                return false;
        }
        return true;
    }

    public bool Craft(Reciepie recipe)
    {
        if (!CanCraft(recipe)) return false;

        foreach (var ingredient in recipe.ingredients)
        {
            playerInventory.RemoveItem(ingredient.item, ingredient.quantity);
        }

        playerInventory.AddItem(recipe.outputItem, recipe.outputQuantity);
        return true;
    }
}