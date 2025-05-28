// Player.cs (or PlayerController.cs, PlayerManager.cs)

using System;
using Player.States;
using UnityEngine;

// For Action (e.g., for general player events like OnPlayerInitialized)

namespace Player // Keep your namespaces consistent!
{
    public class Player : MonoBehaviour
    {
        [Header("Player Systems")] public PlayerMovement _playerMovement;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private HungerSystem _hungerSystem;
        [SerializeField] private Animator animator;
        private PlayerStat _attackStats;
        private PlayerStat _defenseStats;

        private PlayerStat _movementStats;

        private StateMachine _stateMachine;


        [Header("Player Stats")] private ResourceStat HealthStats => _healthSystem.Health;

        private ResourceStat HungerStats => _hungerSystem.Hunger;


        private void Awake()
        {
            InitializeComponents();
            _stateMachine = new StateMachine();

            var idle = new IdleState(this, animator);
            var walk = new WalkState(this, animator);


            _stateMachine.AddAnyTransition(idle, () => _playerMovement.MovementInput.magnitude < 0.1f);
            _stateMachine.AddAnyTransition(walk, () => _playerMovement.MovementInput.magnitude > 0.1f);
            _stateMachine.SetState(idle);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }


        private void InitializeComponents()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _healthSystem = GetComponent<HealthSystem>();
            _hungerSystem = GetComponent<HungerSystem>();
            _movementStats = new PlayerStat(_playerMovement.moveSpeed);
            _attackStats = new PlayerStat(5);
            _defenseStats = new PlayerStat(0);

            _healthSystem?.Initialize(100);
            _hungerSystem?.Initialize(150);
        }


        public PlayerStat GetStat(StatType statType)
        {
            return statType switch
            {
                StatType.Health => HealthStats,
                StatType.Hunger => HungerStats,

                _ => null
            };
        }

        public void TakeDamage(float amount)
        {
            _healthSystem?.TakeDamage(amount);
        }

        public void Eat(float amount)
        {
            _hungerSystem?.Eat(amount);
        }

        private void At(IState from, IState to, Func<bool> condition)
        {
            _stateMachine.AddTransition(from, to, condition);
        }
    }
}