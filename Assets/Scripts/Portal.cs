using UnityEngine;

public class Portal : Structure
{
    public override bool CanInteract(GameObject interactor, Item itemUsed = null)
    {
        return !GameManager.Instance.IsFinished;
    }

    public override void Interact(GameObject interactor, Item itemUsed = null)
    {
        GameManager.Instance.FinishGame();
    }
}