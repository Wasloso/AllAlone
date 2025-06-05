using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewStructureRecipe", menuName = "Recipes/New Structure Recipe")]
public class StructureRecipe: Recipe
{
    public GameObject structurePrefab;

    public override string Name => structurePrefab.name;
    public override Sprite Icon => structurePrefab.GetComponentInChildren<SpriteRenderer>()?.sprite;

}