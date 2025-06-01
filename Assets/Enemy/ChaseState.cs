using UnityEngine;

namespace Enemy
{
    public class ChaseState : IState
    {
        private readonly Animator _animator;
        private readonly Enemy _enemy;

        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private readonly int _X = Animator.StringToHash("X");
        private readonly int _Z = Animator.StringToHash("Z");
        private IDamageable _target;

        public ChaseState(Enemy enemy, Animator animator)
        {
            _enemy = enemy;
            _animator = animator;
        }

        public void OnEnter()
        {
            _target = _enemy.CurrentTarget;
            _animator.SetBool(_isWalking, true);
        }

        public void Tick()
        {
            if (_target == null || !_target.IsAlive)
                return;

            var direction = _target.Transform.position - _enemy.transform.position;
            direction.y = 0;
            direction.Normalize();

            _enemy.transform.position += direction * _enemy.speed * Time.deltaTime;

            _animator.SetFloat(_X, direction.x);
            _animator.SetFloat(_Z, direction.z);
        }

        public void OnExit()
        {
            _animator.SetBool(_isWalking, false);
        }
    }
}