using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewReciepie", menuName = "Reciepies/New Reciepie")]
public class Reciepie : ScriptableObject
{
    public List<IngredientEntry> ingredients;
    public Item outputItem;
    public int outputQuantity = 1;
}