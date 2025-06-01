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
        public bool HasAttacked;

        public AttackState(Enemy enemy, Animator animator)
        {
            _enemy = enemy;
            _animator = animator;
        }

        public void OnEnter()
        {
            _target = _enemy.CurrentTarget;
            _lastAttackTime = -_attackCooldown;
            HasAttacked = false;
        }

        public void Tick()
        {
            if (_target == null || !_target.IsAlive)
                return;


            if (!(Time.time - _lastAttackTime >= _attackCooldown)) return;
            if (DealDamage())
            {
                _lastAttackTime = Time.time;
                HasAttacked = true;
                return;
            }

            HasAttacked = false;
        }

        public void OnExit()
        {
            HasAttacked = false;
        }

        public bool DealDamage()
        {
            if (_target != null && Vector3.Distance(_enemy.transform.position, _target.Transform.position) <=
                _enemy.attackRange)
            {
                _target.TakeDamage(_enemy.attackDamage);
                return true;
            }

            return false;
        }
    }
}