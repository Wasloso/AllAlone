using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask interactableLayer;
    public float interactRadius = 1f;
    private readonly float interactDistance = 10f;

    private PlayerMovement _playerMovement;
    private PlayerInputHandler _inputHandler;
    private InputAction _interactAction;


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        _interactAction = _inputHandler.InputActions.Player.Interact;
        _interactAction.Enable();
        _interactAction.performed += OnKeyboardInteract;
    }

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

    private void OnKeyboardInteract(InputAction.CallbackContext context)
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactDistance, interactableLayer);
        if (interactables.Length == 0) return;

        Collider closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (var interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        if ( closestInteractable != null)
        {
            var interactable = closestInteractable.GetComponent<IInteractable>();
            _playerMovement.MoveToInteractable(closestInteractable.transform.position, interactRadius, () => interactable?.Interact(gameObject));

        }

    }

    public void EquipTool(string tool)
    {
        Debug.Log($"Equipped: {tool}");
    }
}