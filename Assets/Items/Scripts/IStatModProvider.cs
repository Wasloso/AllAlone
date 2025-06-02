using System.Collections.Generic;

public interface IStatModProvider
{
    List<StatModifier> CompiledModifiers { get; }
}