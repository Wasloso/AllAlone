using UnityEngine;

namespace Enemy
{
    public class AttackState : IState
    {
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private readonly Animator _animator;

        private readonly float _attackCooldown;
        private readonly Enemy _enemy;

        private float _attackAnimationLength;
        private float _attackAnimationTimer;
        private bool _isAttacking;
        private float _lastAttackTime;
        private IDamageable _target;
        public bool Finished;

        public AttackState(Enemy enemy, Animator animator, float attackCooldown = 5f)
        {
            _enemy = enemy;
            _animator = animator;
            _attackCooldown = attackCooldown;
            _lastAttackTime = -_attackCooldown;
        }

        public bool CheckAttackTimer => Time.time - _lastAttackTime >= _attackCooldown;

        public void OnEnter()
        {
            _target = _enemy.CurrentTarget;
            _isAttacking = false;
            _attackAnimationTimer = 0f;

            var clips = _animator.GetCurrentAnimatorClipInfo(0);
            foreach (var clipInfo in clips)
                if (clipInfo.clip.name == "Attack")
                {
                    _attackAnimationLength = clipInfo.clip.length;
                    break;
                }

            Finished = false;
        }

        public void Tick()
        {
            if (_target == null || !_target.IsAlive)
                return;


            if (_isAttacking)
            {
                _attackAnimationTimer += Time.deltaTime;
                if (_attackAnimationTimer >= _attackAnimationLength)
                {
                    DealDamage();
                    _isAttacking = false;
                    Finished = true;
                }
            }
            else if (CheckAttackTimer)
            {
                if (CanAttack())
                {
                    _animator.SetTrigger(AttackTrigger);
                    _isAttacking = true;
                    _attackAnimationTimer = 0f;
                    _lastAttackTime = Time.time;
                }
            }
        }

        public void OnExit()
        {
            _isAttacking = false;
            _animator.ResetTrigger(AttackTrigger);
        }

        private bool CanAttack()
        {
            if (_target != null && Vector3.Distance(_enemy.transform.position, _target.Transform.position) <=
                _enemy.attackRange)
                return true;
            return false;
        }

        public bool DealDamage()
        {
            if (CanAttack())
            {
                _target.TakeDamage(_enemy.attackDamage);
                return true;
            }

            return false;
        }
    }
}