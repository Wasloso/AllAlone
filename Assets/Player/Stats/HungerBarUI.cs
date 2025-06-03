using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HungerBarUI : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI hungerText;

    [Header("Dependencies")] [SerializeField]
    private Player.Player playerRef;

    private HungerSystem _hungerSystem;

    private void Awake()
    {
        if (!playerRef) playerRef = FindFirstObjectByType<Player.Player>();

        if (!playerRef)
        {
            Debug.LogError("HealthBarUI: Player reference not found! Disabling UI updates.", this);
            enabled = false;
            return;
        }


        _hungerSystem = playerRef.GetComponent<HungerSystem>();
        if (!_hungerSystem)
        {
            Debug.LogError("HealthBarUI: HealthSystem component not found on Player! Disabling UI updates.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        _hungerSystem.OnHungerChanged += UpdateHungerDisplay;
        UpdateHungerDisplay(_hungerSystem.Hunger.CurrentValue, _hungerSystem.MaxHunger);
    }

    private void OnDestroy()
    {
        if (_hungerSystem != null) _hungerSystem.OnHungerChanged -= UpdateHungerDisplay;
    }


    private void UpdateHungerDisplay(float currentHealth, float maxHealth)
    {
        if (hungerText)
            hungerText.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";
    }


    private void OnPlayerDied()
    {
        Debug.Log("HealthBarUI detected Player Died!");
    }
}