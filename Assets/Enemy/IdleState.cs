using UnityEngine;

namespace Enemy
{
    public class IdleState : IState
    {
        private readonly Animator _animator;
        private readonly Enemy _enemy;
        private readonly int _isWalking = Animator.StringToHash("isWalking");

        public IdleState(Enemy enemy, Animator animator)
        {
            _animator = animator;
            _enemy = enemy;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _enemy.ModifyBoredTimer(start: true, reset: true);
        }

        public void OnExit()
        {
            _enemy.ModifyBoredTimer(true, reset: true);
        }
    }
}