using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewReciepie", menuName = "Reciepies/New Reciepie")]
public class Reciepie : ScriptableObject
{
    public Item outputItem;
    public List<IngredientEntry> ingredients;
}