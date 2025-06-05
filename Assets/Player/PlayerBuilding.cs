using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding: MonoBehaviour
{
    private PlayerInputHandler _inputHandler;

    private InputAction _cancelAction;

    private InputAction _placeAction;
    private GameObject _buildingPrefab;
    private GameObject _previewObject;

    private bool _isBuilding = false;


    public void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    public void Start()
    {
        _placeAction = _inputHandler.InputActions.Building.Place;
        _cancelAction = _inputHandler.InputActions.Building.Cancel;


    }

    public void EnterBuildingMode(GameObject toBuild)
    {
        if (toBuild == null)
        {
            return;
        }
        EnableBuilding();
        _buildingPrefab = toBuild;
        _previewObject = Instantiate(toBuild);
        SetPreviewMode(_previewObject, true);
    }

    private void EnableBuilding()
    {
        _placeAction.performed += OnPlace;
        _cancelAction.performed += OnCancel;
        _isBuilding = true;
        _placeAction.Enable();
        _cancelAction.Enable();
    }

    private void DisableBuilding()
    {
        Destroy(_previewObject);
        _previewObject = null;
        _isBuilding = false;
        _buildingPrefab = null;
        _placeAction.performed -= OnPlace;
        _cancelAction.performed -= OnCancel;
        _placeAction.Disable();
        _cancelAction.Disable();
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        Debug.Log("Placed structure");
        Instantiate(_buildingPrefab,_previewObject.transform.position,Quaternion.identity);
        DisableBuilding();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        DisableBuilding();
    }

    public void Update()
    {
        if (!_isBuilding) return;

        Vector3 worldPos = GetMouseWorldPosition();
        _previewObject.transform.position = SnapToGrid(worldPos);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.point;

        return Vector3.zero;
    }
    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), 0, Mathf.Round(position.z));
    }

    private void SetPreviewMode(GameObject obj, bool active)
    {
        // np. ustaw materia³ przezroczysty
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            r.material.color = active ? new Color(1, 1, 1, 0.5f) : Color.white;
        }
    }
}