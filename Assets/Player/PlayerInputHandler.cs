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
        }

        private void OnDisable()
        {
            InputActions.Player.Disable();
        }

        private void OnDestroy()
        {
            InputActions.Dispose();
        }
    }
}