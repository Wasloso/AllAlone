using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask interactableLayer;
    public float interactRadius = 1f;
    private readonly float searchForInteracablesDistance = 10f;
    private InputAction _clickAction;
    private PlayerInputHandler _inputHandler;
    private InputAction _interactAction;
    private bool _isPointerOverUI;
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
        _clickAction = _inputHandler.InputActions.Player.Click;
        _clickAction.Enable();
        _clickAction.performed += OnClickPerformed;
    }

    private void Update()
    {
        _isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void TryInteract()
    {
        Target = null;

        Vector2 screenPosition;

#if UNITY_IOS || UNITY_ANDROID
    if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
        return;

    screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
#else
        if (Mouse.current == null || !Mouse.current.leftButton.isPressed)
            return;

        screenPosition = Mouse.current.position.ReadValue();
#endif

        var ray = Camera.main.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        if (!Physics.Raycast(ray, out var hitInfo, float.MaxValue, interactableLayer))
            return;

        var distance = Vector3.Distance(transform.position, hitInfo.point);

        if (hitInfo.collider.TryGetComponent<IInteractable>(out var interactable) &&
            interactable.CanInteract(gameObject, _playerInventory.GetEquippedItem(SlotTag.Hand)))
            Target = hitInfo.collider.gameObject;
    }

    private void OnKeyboardInteract(InputAction.CallbackContext context)
    {
        var interactables = Physics.OverlapSphere(transform.position, searchForInteracablesDistance, interactableLayer);
        if (interactables.Length == 0) return;

        Collider closestInteractable = null;
        var closestDistance = float.MaxValue;

        foreach (var interactable in interactables)
        {
            if (!interactable.TryGetComponent<IInteractable>(out var interactableObj)) continue;
            if (!interactableObj.CanInteract(gameObject, _playerInventory.GetEquippedItem(SlotTag.Hand))) continue;

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

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (!_isPointerOverUI) TryInteract();
    }
}