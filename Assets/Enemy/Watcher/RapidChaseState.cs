using UnityEngine;

namespace Enemy.Watcher
{
    public class RapidChaseState : IState
    {
        private readonly Animator _animator;
        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private readonly Watcher _watcher;
        private readonly int _X = Animator.StringToHash("X");
        private readonly int _Z = Animator.StringToHash("Z");
        private float _baseSpeed;
        private IDamageable _target;

        public RapidChaseState(Watcher watcher, Animator animator)
        {
            _watcher = watcher;
            _animator = animator;
        }


        public void Tick()
        {
            var direction = _target.Transform.position - _watcher.transform.position;
            direction.y = 0;
            direction.Normalize();

            _watcher.transform.position += direction * _watcher.speed * Time.deltaTime;

            _animator.SetFloat(_X, direction.x);
            _animator.SetFloat(_Z, direction.z);
        }

        public void OnEnter()
        {
            _target = _watcher.CurrentTarget;
            _baseSpeed = _watcher.speed;
            _watcher.speed = _baseSpeed * 1.5f;
        }

        public void OnExit()
        {
            _watcher.speed = _baseSpeed;
        }
    }
}