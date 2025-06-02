using System;
using Player;
using UnityEngine;

public class ResourceStat : PlayerStat
{
    private float _currentValue;

    public ResourceStat(float baseValue, float minValue = 0f) : base(baseValue)
    {
        MinValue = minValue;
        _currentValue = baseValue;
   
    }

    public float CurrentValue
    {
        get => _currentValue;
        private set
        {
            var oldValue = _currentValue;
            _currentValue = Mathf.Clamp(value, MinValue, Value);
            if (!Mathf.Approximately(oldValue, _currentValue)) OnCurrentValueChanged?.Invoke(oldValue, _currentValue);
        }
    }

    public float MinValue { get; }
    public event Action<float, float> OnCurrentValueChanged;

    public void ModifyCurrent(float delta)
    {
        CurrentValue = Mathf.Clamp(CurrentValue + delta, MinValue, Value);
    }

    public void SetCurrent(float value)
    {
        CurrentValue = Mathf.Clamp(value, MinValue, Value);
    }

    public void FullRestore()
    {
        CurrentValue = Value;
    }

    protected override float CalculateFinalValue()
    {
        var newMaxValue = base.CalculateFinalValue();


        var oldCurrentValue = _currentValue;
        _currentValue = Mathf.Clamp(_currentValue, MinValue, newMaxValue);

        if (!Mathf.Approximately(oldCurrentValue, _currentValue))
            OnCurrentValueChanged?.Invoke(oldCurrentValue, _currentValue);
        return newMaxValue;
    }
}