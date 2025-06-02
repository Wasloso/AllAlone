using UnityEngine;

namespace Player.States
{
    public class WalkTowardsTargetState : IState
    {
        private readonly Animator _animator;
        private readonly Player _player;
        private GameObject _target;

        public WalkTowardsTargetState(Player player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        private float MaxDistance => _player.playerInteractions.interactRadius;

        private int _isWalking => Animator.StringToHash("isWalking");
        private int _X => Animator.StringToHash("X");
        private int _Z => Animator.StringToHash("Z");

        public void Tick()
        {
            if (!_player.playerInteractions.Target) return;
            var direction = _player.playerInteractions.Target.transform.position - _player.transform.position;
            if (direction.magnitude <= MaxDistance * 0.5f)
            {
                _animator.SetBool(_isWalking, false);
                return;
            }

            direction.y = 0f;
            direction.Normalize();

            _player.transform.position += direction * _player.speed * Time.deltaTime;
            var localDirection = _player.transform.InverseTransformDirection(direction);

            _animator.SetFloat(_X, localDirection.x);
            _animator.SetFloat(_Z, localDirection.z);
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