using UnityEngine;

public class Structure : WorldObject
{
    public override bool CanInteract(GameObject interactor, Item itemUsed = null)
    {
        return false;
    }

    public override void Interact(GameObject interactor, Item itemUsed = null)
    {
        
    }

}