using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask interactableLayer;
    public float interactRadius = 1f;
    private readonly float searchForInteracablesDistance = 10f;
    private PlayerInputHandler _inputHandler;
    private InputAction _interactAction;
    private PlayerInventory _playerInventory;

    private PlayerMovement _playerMovement;
    public GameObject Target { get; private set; }


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _playerInventory = GetComponent<PlayerInventory>();
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
        Target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * float.MaxValue, Color.red, 1f);

        if (!Physics.Raycast(ray, out var hitInfo, float.MaxValue, interactableLayer))
            return;

        var distance = Vector3.Distance(transform.position, hitInfo.point);

        if (hitInfo.collider.TryGetComponent<IInteractable>(out var interactable)) Target = hitInfo.collider.gameObject;
    }

    private void OnKeyboardInteract(InputAction.CallbackContext context)
    {
        var interactables = Physics.OverlapSphere(transform.position, searchForInteracablesDistance, interactableLayer);
        if (interactables.Length == 0) return;

        Collider closestInteractable = null;
        var closestDistance = float.MaxValue;

        foreach (var interactable in interactables)
        {
            var distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        if (closestInteractable != null) Target = closestInteractable.gameObject;
    }

    public void ClearTarget()
    {
        Target = null;
    }
}