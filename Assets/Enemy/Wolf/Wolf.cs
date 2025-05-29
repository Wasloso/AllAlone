using UnityEngine;

namespace Enemy
{
    public class Wolf : Enemy
    {
        [SerializeField] private float attackDamage;
        [SerializeField] private int maxHealth;
        private IInteractable _attackTarget;
        private HealthSystem _healthSystem;


        private IState walkTowardsState;

        private void Awake()
        {
            _healthSystem = gameObject.GetComponent<HealthSystem>();
            _healthSystem.Initialize(maxHealth);
            StateMachine = new StateMachine();
            walkTowardsState = new WalkTowardsTargetState(this, animator);
            var idleState = new IdleState(this, animator);

            MoveTarget = new Vector3(10, 0, 10);
            At(idleState, walkTowardsState, () => Vector3.Distance(transform.position, MoveTarget) > 0.1f);
            At(walkTowardsState, idleState, () => Vector3.Distance(transform.position, MoveTarget) < 0.1f);
            StateMachine.SetState(idleState);
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            StateMachine.Tick();
        }

        public new void Interact(GameObject interactor, ItemTool toolUsed = null)
        {
            _healthSystem.TakeDamage(5);
        }
    }
}