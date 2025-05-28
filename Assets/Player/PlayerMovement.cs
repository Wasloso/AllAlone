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

        private InputAction _moveAction;
        private Vector2 _movement;
        public Vector2 MovementInput => _movement;

        private void Awake()
        {
            _camera = Camera.main;
            _inputHandler = GetComponent<PlayerInputHandler>();
            _animator = GetComponentInChildren<Animator>();
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

        private void OnEnable()
        {
            _moveAction = _inputHandler.InputActions.Player.Move;
            _moveAction.Enable();

            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled += OnMoveCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _movement = Vector2.zero;
        }
    }
}