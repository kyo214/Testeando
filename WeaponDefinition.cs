using RogueTest.Core.Combat;
using RogueTest.Core.Systems;

namespace RogueTest.Core.Weapons;

public class WeaponDefinition
{
    public string Name { get; set; } = "Weapon";

    public float Damage { get; set; } = 10;

    public DamageType DamageType { get; set; } =
        DamageType.Physical;

    public float AttackCooldown { get; set; } = 1.0f;

    public WeaponAttackType AttackType { get; set; } =
        WeaponAttackType.Direct;

    public float Range { get; set; } = 100;

    public float ProjectileSpeed { get; set; } = 100;

    public float ProjectileLifetime { get; set; } = 5.0f;

    public float ProjectileRadius { get; set; } = 10.0f;

    public int ProjectilePierce { get; set; } = 0;

    public int ProjectileCount { get; set; } = 1;

    public float ProjectileSpread { get; set; } = 0;

    public TargetingMode TargetingMode { get; set; } =
        TargetingMode.Nearest;
}