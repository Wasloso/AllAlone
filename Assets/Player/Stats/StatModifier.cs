using System;

public enum StatModifierType
{
    Flat,
    Percent,
    Replacement
}

public class StatModifier
{
    public readonly StatType AffectedStat;
    public readonly int Order;
    public readonly object Source;
    public readonly StatModifierType Type;
    public readonly float Value;

    public StatModifier(float value, StatModifierType type, int order, StatType affectedStat, object source = null)
    {
        Value = value;
        Type = type;
        Order = order;
        AffectedStat = affectedStat;
        Source = source;
    }

    public StatModifier(float value, StatModifierType type, StatType affectedStat, object source = null)
        : this(value, type, (int)type, affectedStat, source)
    {
    }
}

[Serializable]
public struct StatModifierEntry
{
    public StatType statType;
    public StatModifierType type;
    public float value;
    public int order;
}