using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public GameObject player;
    public List<Recipe> reciepes;
    private PlayerInventory playerInventory;


    public void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
        if (playerInventory == null) Debug.LogError("PlayerInventory component not found on player object.");
        reciepes = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes"));
    }

    public bool CanCraft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            var owned = playerInventory.CountItem(ingredient.item);
            if (owned < ingredient.quantity)
                return false;
        }

        return true;
    }

    public bool Craft(Recipe recipe)
    {
        if (!CanCraft(recipe)) return false;

        foreach (var ingredient in recipe.ingredients) playerInventory.RemoveItem(ingredient.item, ingredient.quantity);

        if (recipe is ItemRecipe itemRecipie)
        {
            playerInventory.AddItem(itemRecipie.resultItem, itemRecipie.resultAmount);
            return true;
        }

        if (recipe is StructureRecipe placeableRecipie)
        {
            Debug.Log("Crafting structure");
            player.GetComponent<PlayerBuilding>().EnterBuildingMode(placeableRecipie.structurePrefab);
            return true;
        }

        return false;
    }
}