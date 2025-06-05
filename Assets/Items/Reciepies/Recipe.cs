using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipes/New Recipe")]
public abstract class Recipe : ScriptableObject
{
    public List<IngredientEntry> ingredients;

    public abstract string Name { get; }
    public abstract Sprite Icon { get; }
}