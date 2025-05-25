using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask interactableLayer;
    private readonly float interactDistance = 50f;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
            TryInteract();
    }

    private void TryInteract()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 1f);

        if (!Physics.Raycast(ray, out var hitInfo, interactDistance, interactableLayer)) return;
        var interactable = hitInfo.collider.GetComponent<InteractableObject>();

        if (!interactable) return;
        var canInteract = interactable.CheckInteractionDistance(gameObject);
        if (canInteract) interactable.Interact(gameObject);
        else
            //TODO: do something else if cant interact based on distance
            Debug.Log("Too far away, move closer!");
    }

    public void EquipTool(string tool)
    {
        Debug.Log($"Equipped: {tool}");
    }
}