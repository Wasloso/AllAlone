using UnityEngine;

namespace  Player
{
    
    public class PlayerInputHandler : MonoBehaviour
    {

        public PlayerInputActions InputActions { get; private set; }

        void Awake()
        {
            InputActions = new PlayerInputActions();
        }

        void OnEnable()
        {
            InputActions.Player.Enable();
        }

        void OnDisable()
        {
            InputActions.Player.Disable();
        }

        void OnDestroy()
        {
            InputActions.Dispose();
        }
    }
}
