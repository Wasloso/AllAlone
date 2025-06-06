using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;

        private Animator _animator;
        private Camera _camera;
        private PlayerInputHandler _inputHandler;
        private Vector3 _interactablePosition;
        private float _interactableRadius;

        private bool _isMovingToInteractable;

        private InputAction _moveAction;
        private Vector2 _movement;
        private Action _onReachTarget;
        public Vector2 MovementInput => _movement;

        private void Awake()
        {
            _camera = Camera.main;
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            EnableActions();
        }


        private void Update()
        {
            var cameraForward = _camera.transform.forward;
            var cameraRight = _camera.transform.right;


            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();


            var moveDirection = (cameraRight * _movement.x + cameraForward * _movement.y).normalized;


            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }

        private void EnableActions()
        {
            _moveAction = _inputHandler.PlayerMovementAction;
            _moveAction.Enable();

            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled += OnMoveCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            if (Time.timeScale == 0f) return;
            _isMovingToInteractable = false;
            _movement = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            if (Time.timeScale == 0f) return;
            _isMovingToInteractable = false;
            _movement = Vector2.zero;
        }

        internal void MoveToInteractable(Vector3 interactablePosition, float interactableRadius, Action onReachTarget)
        {
            Debug.Log($"Moving to interactable at {interactablePosition} with radius {interactableRadius}");
            _isMovingToInteractable = true;
            _interactablePosition = interactablePosition;
            _interactableRadius = interactableRadius;
            _onReachTarget = onReachTarget;
        }
    }
}