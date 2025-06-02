using System.Collections;
using UnityEngine;

namespace Player.States
{
    public class InteractState : IState
    {
        private static readonly int _attackTrigger = Animator.StringToHash("Attack");
        private readonly Animator _animator;
        private readonly Player _player;


        public InteractState(Player player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        private float MaxDistance => _player.playerInteractions.interactRadius;

        private IInteractable Interactable => _player.playerInteractions.Target.GetComponent<IInteractable>();

        public void Tick()
        {
        }

        public void OnEnter()
        {
            if (!_player.playerInteractions.Target) return;
            if (Interactable == null) return;
            var distance = Vector3.Distance(_player.transform.position,
                _player.playerInteractions.Target.transform.position);
            if (distance > MaxDistance) return;
            var itemUsed = _player._playerInventory.GetEquippedItem(SlotTag.Hand);
            if (!Interactable.CanInteract(_player.gameObject, itemUsed)) return;
            switch (Interactable.AnimationType)
            {
                case InteractionAnimationType.Attack:
                {
                    _animator.SetTrigger(_attackTrigger);
                    break;
                }
            }

            Interactable.Interact(_player.gameObject, itemUsed);
            _player.playerInteractions.ClearTarget();
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