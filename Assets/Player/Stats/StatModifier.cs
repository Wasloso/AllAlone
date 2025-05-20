public enum StatModifierType
{
    Flat,
    Percent,
    Replacement,
}
public class StatModifier
{
    public readonly float Value;
    public readonly StatModifierType Type;
    public readonly int Order;
    public readonly StatType AffectedStat;

    public StatModifier(float value, StatModifierType type, int order, StatType affectedStat)
    {
        Value = value;
        Type = type;
        Order = order;
        AffectedStat = affectedStat;
        
    }
    
    public StatModifier(float value, StatModifierType type, StatType affectedStat) : this(value, type, (int)type, affectedStat) { }
    
}

[System.Serializable]
public struct StatModifierEntry
{
    public StatType statType; 
    public StatModifierType type;
    public float value;
    public int order;
}