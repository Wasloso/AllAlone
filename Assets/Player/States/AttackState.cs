using System.Collections;
using UnityEngine;

namespace Player.States
{
    public class AttackState : IState
    {
        private static readonly int _attackTrigger = Animator.StringToHash("Attack");
        private readonly Animator _animator;
        private readonly Player _player;
        private bool _hasAttacked;


        public AttackState(Player player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        private float MaxDistance => _player.playerInteractions.interactRadius;

        private IDamageable Interactable => _player.playerInteractions.Target.GetComponent<IDamageable>();

        public void Tick()
        {
            if (!_player.playerInteractions.Target) return;
            if (Interactable == null) return;
            if (_hasAttacked) return;
            var distance = Vector3.Distance(_player.transform.position,
                _player.playerInteractions.Target.transform.position);
            if (distance > MaxDistance) return;
            _animator.SetTrigger(_attackTrigger);
            var damageAmout = _player.GetStat(StatType.Attack).Value;
            Interactable.TakeDamage(damageAmout);
            Debug.Log($"Attacked {_player.playerInteractions.Target.name} for {damageAmout} damage!");
            _hasAttacked = true;
            _player.playerInteractions.ClearTarget();
        }

        public void OnEnter()
        {
            _hasAttacked = false;
            Debug.Log("Enter Attack State");
        }

        public void OnExit()
        {
            _animator.ResetTrigger(_attackTrigger);
            _player.playerInteractions.ClearTarget();
        }

        private IEnumerator EndAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}