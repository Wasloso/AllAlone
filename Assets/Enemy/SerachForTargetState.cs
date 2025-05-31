using UnityEngine;

namespace Enemy
{
    public class SerachForTargetState : IState
    {
        private readonly Enemy _enemy;

        public SerachForTargetState(Enemy enemy
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
            Debug.Log("SerachForTargetState.OnEnter");
            _enemy.FindNewTarget();
        }

        public void OnExit()
        {
        }
    }
}