// Player.cs (or PlayerController.cs, PlayerManager.cs)

using UnityEngine;

// For Action (e.g., for general player events like OnPlayerInitialized)

namespace Player // Keep your namespaces consistent!
{
    public class Player : MonoBehaviour
    {
        // Public references or [SerializeField] for easy assignment in Unity Editor
        [Header("Player Systems")] [SerializeField]
        private PlayerMovement _playerMovement;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private HungerSystem _hungerSystem;
        private PlayerStat _attackStats;
        private PlayerStat _defenseStats;
        private PlayerStat _movementStats;

        [Header("Player Stats")] private ResourceStat HealthStats => _healthSystem.Health;

        private ResourceStat HungerStats => _hungerSystem.Hunger;


        private void Awake()
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

        // --- Public API for Other Systems to Interact with Player ---

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
    }
}