using System;
using Items;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HungerSystem : MonoBehaviour, IConsumableReciever
{
    [Header("Hunger Settings")] [SerializeField]
    private float decayRatePerSecond = 1f;

    [SerializeField] private float starvationDamagePerSecond = 5f;

    private HealthSystem _healthSystem;
    public ResourceStat Hunger { get; private set; }

    public float MaxHunger => Hunger.Value;
    public float CurrentHunger => Hunger.CurrentValue;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        if (_healthSystem == null)
            Debug.LogError(
                "HungerSystem: HealthSystem reference not found on this GameObject! Starvation damage will not work.",
                this);
    }

    private void Update()
    {
        if (decayRatePerSecond == 0) return;
        Hunger.ModifyCurrent(-decayRatePerSecond * Time.deltaTime);

        if (CurrentHunger <= 0) _healthSystem.TakeDamage(starvationDamagePerSecond * Time.deltaTime);
    }

    public void Consume(ItemConsumable consumable)
    {
        Hunger.ModifyCurrent(consumable.restoreHunger);
    }

    public event Action<float, float> OnHungerChanged;


    public void Initialize(float baseHunger)
    {
        if (Hunger == null)
        {
            Hunger = new ResourceStat(baseHunger);
            Debug.LogWarning("Hunger stats not initialized!", this);
        }
        else
        {
            Hunger.BaseValue = baseHunger;
            Hunger.SetCurrent(baseHunger);
        }

        Hunger.OnCurrentValueChanged += HandleCurrentHungerChanged;
        Debug.Log($"HungerSystem Initialized. Base Hunger: {baseHunger}", this);
    }


    private void HandleCurrentHungerChanged(float oldCurrentValue, float newCurrentValue)
    {
        OnHungerChanged?.Invoke(newCurrentValue, MaxHunger);
    }

    private void HandleMaxHungerChanged(float oldMaxValue, float newMaxValue)
    {
        OnHungerChanged?.Invoke(CurrentHunger, newMaxValue);
    }
}

public interface IConsumableReciever
{
    public void Consume(ItemConsumable consumable);
}