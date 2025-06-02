using UnityEngine;

namespace Enemy
{
    public class Wolf : Enemy
    {
        private IInteractable _attackTarget;


        protected new void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
            var walkTowardsState = new WalkTowardsTargetState(this, animator);
            var idleState = new IdleState(this, animator);
            var searchForTargetWanderAroundState = new SearchForTargetWanderAroundState(this);
            var searchForAttackTargetState = new SearchForAttackTargetState();
            var chaseState = new ChaseState(this, animator);
            var attackState = new AttackState(this, animator);
            var deathState = new DeathState(this, animator, Die);

            At(idleState, searchForTargetWanderAroundState, () => boredTimer < 0f);
            At(idleState, chaseState, () => CheckForTarget());
            At(searchForTargetWanderAroundState, walkTowardsState,
                () => Vector3.Distance(transform.position, MoveTarget) > 0.1f);
            At(walkTowardsState, idleState, () => Vector3.Distance(transform.position, MoveTarget) < 0.1f);
            At(walkTowardsState, chaseState, () => CheckForTarget());
            At(chaseState, attackState, () => IsTargetInRange() && attackState.CheckAttackTimer);
            At(attackState, chaseState, () => attackState.Finished);
            At(chaseState, idleState, () => !CheckForTarget(1.5f));
            _healthSystem.OnDied += () => { StateMachine.SetState(deathState); };
            StateMachine.SetState(idleState);
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            StateMachine.Tick();
            if (updateBoredTimer) boredTimer -= Time.deltaTime;
        }
    }
}