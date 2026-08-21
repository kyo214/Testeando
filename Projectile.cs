using RogueTest.Core.Combat;

namespace RogueTest.Core.Entities;

public class Projectile : Entity
{
    public System.Numerics.Vector2 Velocity { get; set; }

    public DamageInfo Damage { get; }

    public CharacterEntity Owner { get; }

    public float Lifetime { get; set; } = 5.0f;
    public float Radius { get; set; } = 10.0f;
    public int PierceRemaining { get; set; }
    public HashSet<CharacterEntity> HitTargets { get; } = new();
    public Projectile(
        CharacterEntity owner,
        DamageInfo damage)
    {
        Owner = owner;
        Damage = damage;
    }
}