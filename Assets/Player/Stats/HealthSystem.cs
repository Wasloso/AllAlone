using System;
using Items;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IConsumableReciever
{
    public ResourceStat Health { get; private set; }
    public float MaxHealth => Health.Value;
    public float CurrentHealth => Health.CurrentValue;

    private void OnDestroy()
    {
        if (Health != null) Health.OnCurrentValueChanged -= OnHealthChanged;
    }

    public void Consume(ItemConsumable consumable)
    {
        Health.ModifyCurrent(consumable.restoreHealth);
    }

    public event Action<float, float> OnHealthChanged; // (currentHealth, maxHealth)
    public event Action OnDied;

    public void Initialize(float baseHealth)
    {
        Health = new ResourceStat(baseHealth);
        Health.OnCurrentValueChanged += HandleCurrentHealthChanged;
        Debug.Log($"HealthSystem Initialized. Base Health: {baseHealth}, Max Health: {MaxHealth}");
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        Health.ModifyCurrent(-amount);
        if (Health.CurrentValue <= 0) Die();
    }


    private void Die()
    {
        OnDied?.Invoke();
    }

    private void HandleCurrentHealthChanged(float oldCurrentValue, float newCurrentValue)
    {
        OnHealthChanged?.Invoke(newCurrentValue, MaxHealth);
    }

    private void HandleMaxHealthChanged(float oldMaxValue, float newMaxValue)
    {
        OnHealthChanged?.Invoke(CurrentHealth, newMaxValue);
    }
}