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
            Debug.Log($"{_enemy.name} is idle");
        }

        public void OnExit()
        {
        }
    }
}