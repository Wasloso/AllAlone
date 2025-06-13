
using Items;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject: MonoBehaviour, IInteractable
{
    public GameObject droppedItemPrefab;
    [SerializeField] protected InteractionAnimationType interactionAnimationType = InteractionAnimationType.None;
    public float currentHealth = 10f;
    public List<ItemDropEntry> itemDrops;

    public InteractionAnimationType AnimationType => interactionAnimationType;
    public abstract bool CanInteract(GameObject gameObject, Item item);
    public abstract void Interact(GameObject gameObject, Item item);
    

}