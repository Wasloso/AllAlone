using UnityEngine;

namespace Enemy
{
    public class WalkTowardsTargetState : IState
    {
        private readonly Animator _animator;
        private readonly Enemy _enemy;

        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private readonly float _waitDuration = 2f;
        private readonly int _X = Animator.StringToHash("X");
        private readonly int _Z = Animator.StringToHash("Z");
        private bool _isWaiting;

        private float _waitTimer;

        public WalkTowardsTargetState(Enemy enemy, Animator animator)
        {
            _enemy = enemy;
            _animator = animator;
        }

        public void OnEnter()
        {
            _animator.SetBool(_isWalking, true);
        }

        public void Tick()
        {
            var direction = _enemy.MoveTarget - _enemy.transform.position;
            direction.y = 0f;
            direction.Normalize();
#if UNITY_EDITOR
            Debug.Log(direction);
#endif

            _enemy.transform.position += direction * _enemy.speed * Time.deltaTime;

            _animator.SetFloat(_X, direction.x);
            _animator.SetFloat(_Z, direction.z);
            _animator.SetBool(_isWalking, true);
        }

        public void OnExit()
        {
            _animator.SetBool(_isWalking, false);
        }
    }
}