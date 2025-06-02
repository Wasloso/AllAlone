using UnityEngine;

namespace Player.States
{
    public class IdleState : IState
    {
        private readonly Animator _animator;
        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private readonly Player _player;

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
            _player.playerInteractions.ClearTarget();
        }

        public void OnExit()
        {
        }
    }
}