using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStat
    {
        private readonly List<StatModifier> _modifiers;
        private bool _isDirty = true;
        private float _lastValue;
        public float BaseValue;

        public PlayerStat(float baseValue)
        {
            BaseValue = baseValue;
            _modifiers = new List<StatModifier>();
        }

        public float Value
        {
            get
            {
                if (!_isDirty) return _lastValue;
                var oldValue = _lastValue;
                _lastValue = CalculateFinalValue();
                _isDirty = false;
                if (!Mathf.Approximately(oldValue, _lastValue)) OnValueChanged?.Invoke(oldValue, _lastValue);

                return _lastValue;
            }
            set => _lastValue = value;
        }

        public event Action<float, float> OnValueChanged;

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            _isDirty = true;
            _modifiers.Sort((m1, m2) => m1.Order.CompareTo(m2.Order));
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier);
            _isDirty = true;
        }

        protected virtual float CalculateFinalValue()
        {
            var replacementValue = float.MinValue;
            var flatSum = 0f;
            var percentSum = 0f;

            foreach (var modifier in _modifiers)
                switch (modifier.Type)
                {
                    case StatModifierType.Replacement:
                        if (modifier.Value > replacementValue)
                            replacementValue = modifier.Value;
                        break;

                    case StatModifierType.Flat:
                        flatSum += modifier.Value;
                        break;

                    case StatModifierType.Percent:
                        percentSum += modifier.Value;
                        break;
                }

            var finalValue = replacementValue > float.MinValue ? replacementValue : BaseValue;

            finalValue += flatSum;
            finalValue *= 1 + percentSum;

            return (float)Math.Round(finalValue, 4);
        }

        public void RemoveAllModifiersFromSource(object source)
        {
            if (source == null) return;

            _modifiers.RemoveAll(modifier => modifier.Source == source);
            _isDirty = true;
        }
    }
}

public enum StatType
{
    Health,
    Hunger,
    Sanity,
    Defense,
    Attack,
    Movement
}