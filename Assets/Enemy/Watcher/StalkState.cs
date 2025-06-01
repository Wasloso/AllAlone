using UnityEngine;

namespace Enemy.Watcher
{
    public class StalkState : IState
    {
        private readonly Animator _animator;

        private readonly int _isWalking = Animator.StringToHash("isWalking");
        private readonly Watcher _watcher;
        private readonly int _X = Animator.StringToHash("X");
        private readonly int _Z = Animator.StringToHash("Z");
        private float _baseSpeed;
        private IDamageable _target;


        public StalkState(Watcher watcher, Animator animator)
        {
            _watcher = watcher;
            _animator = animator;
        }

        public bool IsStalking { get; private set; }

        public void Tick()
        {
            if (_watcher.CurrentTarget == null || !_watcher.CurrentTarget.IsAlive)
            {
                IsStalking = false;
                _animator.SetBool(_isWalking, false);
                _animator.SetFloat(_X, 0);
                _animator.SetFloat(_Z, 0);
                return;
            }

            var distanceFromTarget =
                Vector3.Distance(_watcher.transform.position, _watcher.CurrentTarget.Transform.position);
            Debug.Log($"Distance: {distanceFromTarget}");
            if (distanceFromTarget < _watcher.stalkRange / 2f)
            {
                var direction = _target.Transform.position - _watcher.transform.position;
                direction.y = 0;
                direction.Normalize();

                _watcher.transform.position -= direction * _watcher.speed * Time.deltaTime;

                _animator.SetFloat(_X, direction.x);
                _animator.SetFloat(_Z, direction.z);
                IsStalking = true;
                return;
            }

            if (distanceFromTarget > _watcher.stalkRange * 2f)
            {
                IsStalking = false;
                return;
            }


            if (distanceFromTarget > _watcher.stalkRange * 1.2f)
            {
                var direction = _target.Transform.position - _watcher.transform.position;
                direction.y = 0;
                direction.Normalize();

                _watcher.transform.position += direction * _watcher.speed * Time.deltaTime;

                _animator.SetFloat(_X, direction.x);
                _animator.SetFloat(_Z, direction.z);
                IsStalking = true;
                return;
            }


            _watcher.currentStalkTime += Time.deltaTime;
            IsStalking = true;
        }


        public void OnEnter()
        {
            _target = _watcher.CurrentTarget;
            _watcher.currentStalkTime = 0f;
            _baseSpeed = _watcher.speed;
            _watcher.speed = 1f;
            IsStalking = true;
        }

        public void OnExit()
        {
            _watcher.speed = _baseSpeed;
            _watcher.currentStalkTime = 0f;
        }
    }
}