using UnityEngine;

namespace Enemy
{
    public class SearchForTargetWanderAroundState : IState
    {
        private readonly Enemy _enemy;

        public SearchForTargetWanderAroundState(Enemy enemy
        )
        {
            _enemy = enemy;
        }

        public void Tick()
        {
            if (Vector3.Distance(_enemy.transform.position, _enemy.MoveTarget) < 0.1f) _enemy.FindNewTarget();
        }

        public void OnEnter()
        {
            _enemy.FindNewTarget();
        }

        public void OnExit()
        {
        }
    }
}