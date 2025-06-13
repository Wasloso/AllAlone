using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemWithStats : Item, IStatModProvider
{
    [Header("Modifiers")] public List<StatModifierEntry> statModifiers;

    [NonSerialized] public List<StatModifier> compiledModifiers = new();

    public List<StatModifier> CompiledModifiers => compiledModifiers;

    public virtual void ValidateStats()
    {
        CompileModifiers();
    }

    public void CompileModifiers()
    {
        compiledModifiers.Clear();
        foreach (var entry in statModifiers)
        {
            var modifier = new StatModifier(
                entry.value,
                entry.type,
                entry.order == 0 ? (int)entry.type : entry.order,
                entry.statType,
                this
            );
            compiledModifiers.Add(modifier);
        }
    }
}