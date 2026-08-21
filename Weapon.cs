using RogueTest.Core.Combat;
using RogueTest.Core.Systems;

namespace RogueTest.Core.Weapons;

public class Weapon
{
    public string Name { get; set; } = "Weapon";

    public float Damage { get; set; } = 10;

    public DamageType DamageType { get; set; } = DamageType.Physical;

    public float AttackCooldown { get; set; } = 1.0f;

    public float CriticalChance { get; set; } = 0;

    public float CriticalMultiplier { get; set; } = 2;

    public float Range { get; set; } = 100;
    public float CooldownRemaining { get; private set; }
    public WeaponAttackType AttackType { get; set; } = WeaponAttackType.Direct;
    public float ProjectileSpeed { get; set; } = 100;
    public float ProjectileLifetime { get; set; } = 5.0f;

    public float ProjectileRadius { get; set; } = 10.0f;
    public int ProjectilePierce { get; set; } = 0;
    public int ProjectileCount { get; set; } = 1;

    public float ProjectileSpread { get; set; } = 0;
    public TargetingMode TargetingMode { get; set; } =
    TargetingMode.Nearest;
    public bool CanAttack()
    {
        return CooldownRemaining <= 0.001f;
    }

    public void Update(float delta)
    {
        if (CooldownRemaining > 0)
        {
            CooldownRemaining -= delta;

            if (CooldownRemaining < 0.001f)
                CooldownRemaining = 0;
        }
    }

    public void StartCooldown()
    {
        CooldownRemaining = AttackCooldown;
    }
    public void StartCooldown(float attackSpeed)
    {
        if (attackSpeed <= 0)
            attackSpeed = 1;

        CooldownRemaining =
            AttackCooldown / attackSpeed;
    }
}