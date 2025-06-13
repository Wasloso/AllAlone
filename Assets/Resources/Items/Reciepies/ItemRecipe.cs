using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewItemRecipie", menuName = "Recipes/New Item Recipe")]
public class ItemRecipe : Recipe
{
    public Item resultItem;
    public int resultAmount = 1;

    public override string Name => resultItem.title;
    public override Sprite Icon => resultItem.icon;

}