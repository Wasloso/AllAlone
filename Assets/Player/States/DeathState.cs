using System;
using UnityEngine;

namespace Player.States
{
    public class DeathState : IState
    {
        private readonly Animator _animator;

        // private readonly int _die = Animator.StringToHash("Die");
        private readonly Action _onDeath;
        private readonly Player _player;
        private bool _deathStarted;

        public DeathState(Player player, Animator animator, Action onDeath)
        {
            _animator = animator;
            _player = player;
            _onDeath = onDeath;
        }

        public void Tick()
        {
            // if (_deathStarted) return;
            //
            // var animState = _animator.GetCurrentAnimatorStateInfo(0);
            // if (animState.IsName("Die") && animState.normalizedTime >= 1f)
            // {
            //     _deathStarted = true;
            //     _onDeath?.Invoke();
            // }
        }

        public void OnEnter()
        {
            Debug.Log("Death", _player.gameObject);
            _player.DisableComponents();


            // _animator.SetTrigger(_die);
            // _deathStarted = false;
        }

        public void OnExit()
        {
        }
    }
}