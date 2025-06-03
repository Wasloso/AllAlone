using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerInputActions InputActions { get; private set; }
        public InputAction PlayerMovementAction => InputActions.Player.Move;

        private void Awake()
        {
            InputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            InputActions.Player.Enable();
            InputActions.UI.Enable();

            InputActions.UI.Pause.performed += OnPause;
        }

        private void OnDisable()
        {
            InputActions.Player.Disable();
            InputActions.UI.Disable();

            InputActions.UI.Pause.performed -= OnPause;
        }

        private void OnDestroy()
        {
            InputActions.Dispose();
        }

        public event Action OnPausePressed;

        private void OnPause(InputAction.CallbackContext context)
        {
            Debug.Log("OnPause");
            OnPausePressed?.Invoke();
        }
    }
}