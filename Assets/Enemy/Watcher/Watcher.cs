using UnityEngine;

namespace Enemy.Watcher
{
    public class Watcher : Enemy
    {
        [SerializeField] public float stalkTime = 10f;
        public float currentStalkTime;
        public float stalkRange = 10f;
        private IInteractable _attackTarget;


        protected new void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
            var walkTowardsState = new WalkTowardsTargetState(this, animator);
            var idleState = new IdleState(this, animator);
            var searchForTargetWanderAroundState = new SearchForTargetWanderAroundState(this);
            var stalkState = new StalkState(this, animator);
            var attackState = new AttackState(this, animator);
            var rapidChaseState = new RapidChaseState(this, animator);
            var moveAwayAfterAttackState = new MoveAwayAfterAttackState(this, animator);


            At(idleState, searchForTargetWanderAroundState, () => boredTimer < 0f);
            At(idleState, stalkState, () => CheckForTarget());
            At(searchForTargetWanderAroundState, walkTowardsState,
                () => Vector3.Distance(transform.position, MoveTarget) > 0.1f);
            At(walkTowardsState, idleState, () => Vector3.Distance(transform.position, MoveTarget) <= 0.1f);
            At(walkTowardsState, stalkState, () => CheckForTarget());
            At(searchForTargetWanderAroundState, stalkState, () => CheckForTarget());
            At(stalkState, rapidChaseState, () => currentStalkTime >= stalkTime);
            At(rapidChaseState, attackState, IsTargetInRange);
            At(attackState, moveAwayAfterAttackState, () => attackState.HasAttacked);
            At(moveAwayAfterAttackState, stalkState, () => moveAwayAfterAttackState.Completed);
            At(stalkState, idleState, () => !stalkState.IsStalking);
            StateMachine.SetState(idleState);
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            StateMachine.Tick();
            Debug.Log(StateMachine.CurrentState);
            if (updateBoredTimer) boredTimer -= Time.deltaTime;
        }
    }
}