using System;
using UnityEngine;

namespace Enemy
{
    public class DeathState : IState
    {
        private readonly Animator _animator;
        private readonly int _die = Animator.StringToHash("Die");
        private readonly Enemy _enemy;
        private readonly Action _onDeath;
        private bool _deathStarted;

        public DeathState(Enemy enemy, Animator animator, Action onDeath)
        {
            _animator = animator;
            _enemy = enemy;
            _onDeath = onDeath;
        }

        public void Tick()
        {
            if (_deathStarted) return;

            var animState = _animator.GetCurrentAnimatorStateInfo(0);
            if (animState.IsName("Die") && animState.normalizedTime >= 1f)
            {
                _deathStarted = true;
                _onDeath?.Invoke();
            }
        }

        public void OnEnter()
        {
            _animator.SetTrigger(_die);
            _deathStarted = false;
        }

        public void OnExit()
        {
        }
    }
}