using UnityEngine;

public enum InteractionAnimationType
{
    None,
    Pickup,
    Attack,
    Use
}

public interface IInteractable
{
    InteractionAnimationType AnimationType { get; }
    public bool CanInteract(GameObject interactor, Item itemUsed = null);
    public void Interact(GameObject interactor, Item itemUsed = null);
}