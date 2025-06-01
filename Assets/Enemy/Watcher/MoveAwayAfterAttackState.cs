using UnityEngine;

namespace Enemy.Watcher
{
    public class MoveAwayAfterAttackState : IState
    {
        private readonly Animator _animator;

        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private readonly Watcher _watcher;
        private readonly int _X = Animator.StringToHash("X");
        private readonly int _Z = Animator.StringToHash("Z");
        private float _baseSpeed;
        private IDamageable _target;

        public MoveAwayAfterAttackState(Watcher watcher, Animator animator)
        {
            _watcher = watcher;
            _animator = animator;
        }

        public bool Completed { get; private set; }

        public void Tick()
        {
            var direction = _watcher.MoveTarget - _watcher.transform.position;
            direction.y = 0f;
            direction.Normalize();


            _watcher.transform.position += direction * _watcher.speed * Time.deltaTime;

            _animator.SetFloat(_X, direction.x);
            _animator.SetFloat(_Z, direction.z);
            _animator.SetBool(_isWalking, true);
            if (Vector3.Distance(_watcher.transform.position, _watcher.MoveTarget) < 0.1f) Completed = true;
        }

        public void OnEnter()
        {
            _baseSpeed = _watcher.speed;
            _watcher.speed = _baseSpeed * 2;
            _watcher.MoveTarget = _watcher.FindNewTarget();
            Completed = false;
        }

        public void OnExit()
        {
            _watcher.speed = _baseSpeed;
            Completed = true;
        }
    }
}