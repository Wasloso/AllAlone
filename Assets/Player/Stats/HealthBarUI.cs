using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private TextMeshProUGUI healthText;

    [Header("Dependencies")] [SerializeField]
    private Player.Player playerRef;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        if (!playerRef) playerRef = FindFirstObjectByType<Player.Player>();

        if (!playerRef)
        {
            Debug.LogError("HealthBarUI: Player reference not found! Disabling UI updates.", this);
            enabled = false;
            return;
        }


        _healthSystem = playerRef.GetComponent<HealthSystem>();
        if (_healthSystem) return;
        Debug.LogError("HealthBarUI: HealthSystem component not found on Player! Disabling UI updates.", this);
        enabled = false;
    }


    private void Start()
    {
        _healthSystem.OnHealthChanged += UpdateHealthDisplay;
        _healthSystem.OnDied += OnPlayerDied;
        UpdateHealthDisplay(_healthSystem.CurrentHealth, _healthSystem.MaxHealth);
    }

    private void OnDestroy()
    {
        if (_healthSystem)
        {
            _healthSystem.OnHealthChanged -= UpdateHealthDisplay;
            _healthSystem.OnDied -= OnPlayerDied;
        }
    }


    private void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        if (healthText)
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";
    }


    private void OnPlayerDied()
    {
    }
}