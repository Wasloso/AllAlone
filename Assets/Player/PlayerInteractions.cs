using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask interactableLayer;
    public float interactRadius = 5f;
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

        if (!Physics.Raycast(ray, out var hitInfo, interactDistance, interactableLayer))
            return;

        var distance = Vector3.Distance(transform.position, hitInfo.point);
        if (distance > interactRadius)
            return;
        var interactable = hitInfo.collider.GetComponent<IInteractable>();
        interactable?.Interact(gameObject);
    }

    public void EquipTool(string tool)
    {
        Debug.Log($"Equipped: {tool}");
    }
}