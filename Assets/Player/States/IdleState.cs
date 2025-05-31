using UnityEngine;

namespace Player.States
{
    public class IdleState : IState
    {
        private readonly Animator _animator;
        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private Player _player;

        public IdleState(Player player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("Player is idle");
        }

        public void OnExit()
        {
        }
    }
}