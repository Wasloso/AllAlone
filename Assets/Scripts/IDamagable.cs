using UnityEngine;

public interface IDamageable
{
    Transform Transform { get; }
    Faction Faction { get; }
    bool IsAlive { get; }
    void TakeDamage(float amount);
}

public enum Faction
{
    Player,
    Enemy,
    Neutral,
    FriendlyNPC // Maybe?
}