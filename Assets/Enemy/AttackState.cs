using UnityEngine;

namespace Enemy
{
    public class AttackState : IState
    {
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private readonly Animator _animator;

        private readonly float _attackCooldown = 1.5f;
        private readonly Enemy _enemy;
        private float _lastAttackTime;
        private IDamageable _target;

        public AttackState(Enemy enemy, Animator animator)
        {
            _enemy = enemy;
            _animator = animator;
        }

        public void OnEnter()
        {
            _target = _enemy.CurrentTarget;
            _lastAttackTime = -_attackCooldown; // allows immediate first attack
        }

        public void Tick()
        {
            if (_target == null || !_target.IsAlive)
                return;

            // Face the target
            var direction = _target.Transform.position - _enemy.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
                _enemy.transform.forward = direction.normalized;

            if (Time.time - _lastAttackTime >= _attackCooldown)
            {
                DealDamage();
                _lastAttackTime = Time.time;
            }
        }

        public void OnExit()
        {
            // Optional: reset animation triggers
        }

        // Call this from an animation event (e.g., at the hit frame)
        public void DealDamage()
        {
            if (_target != null && Vector3.Distance(_enemy.transform.position, _target.Transform.position) <=
                _enemy.attackRange) _target.TakeDamage(_enemy.attackDamage);
        }
    }
}