using Sounds;
using UnityEngine;

namespace Player.States
{
    public class WalkState : IState
    {
        private readonly Animator _animator;
        private readonly Player _player;


        public WalkState(Player player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        private int _isWalking => Animator.StringToHash("isWalking");
        private int _X => Animator.StringToHash("X");
        private int _Z => Animator.StringToHash("Z");

        public void Tick()
        {
            var input = _player._playerMovement.MovementInput;
            _animator.SetFloat(_X, input.x);
            _animator.SetFloat(_Z, input.y);
            _player.playerInteractions.ClearTarget();
            
        }

        public void OnEnter()
        {
            _animator.SetBool(_isWalking, true);
        }

        public void OnExit()
        {
            _animator.SetBool(_isWalking, false);
        }
    }
}