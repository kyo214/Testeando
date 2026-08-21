using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Weapons;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class WeaponSystem
{
    private readonly TargetingSystem targetingSystem =
    new TargetingSystem();
    public int DebugAttackCount { get; private set; }

    public int DebugEventCount { get; private set; }

    public int DebugPlayerAttackCount { get; private set; }
    public int DebugPlayerEventCount { get; private set; }
    public int DebugPlayerAttacks { get; private set; }
    public int DebugPlayerEvents { get; private set; }
    public int DebugWeaponEvents { get; private set; }
    public List<GameEvent> Update(
    CharacterEntity attacker,
    GameWorld world,
    CombatSystem combat,
    float delta)
    {
        if (attacker is Player)
        {
            DebugPlayerAttacks = 0;
            DebugPlayerEvents = 0;
        }

        List<GameEvent> events = new();


        foreach (Weapon weapon in attacker.Weapons)
        {
            weapon.Update(delta);


            if (attacker is Enemy enemy &&
                enemy.AIState != EnemyAIState.Attack)
            {
                continue;
            }


            if (!weapon.CanAttack())
                continue;


            CharacterEntity? target =
                targetingSystem.FindTarget(
                    attacker,
                    world,
                    weapon.Range,
                    weapon.TargetingMode);


            if (target == null)
                continue;


            if (attacker is Player)
            {
                DebugPlayerAttacks++;
            }


            switch (weapon.AttackType)
            {
                case WeaponAttackType.Direct:

                    DamageInfo damage =
                        combat.CreateDamage(
                            attacker,
                            weapon);


                    DamageResult result =
                        combat.Attack(
                            attacker,
                            target,
                            damage);


                    events.Add(
                        new DamageEvent(
                            attacker,
                            target,
                            result));


                    if (result.TargetDied)
                    {
                        events.Add(
                            new DeathEvent(target));
                    }


                    break;


                case WeaponAttackType.Projectile:

                    System.Numerics.Vector2 baseDirection =
                        target.Position - attacker.Position;


                    if (baseDirection.LengthSquared() > 0)
                    {
                        baseDirection =
                            System.Numerics.Vector2.Normalize(
                                baseDirection);
                    }


                    int projectileCount =
                        Math.Max(1, weapon.ProjectileCount);


                    for (int i = 0; i < projectileCount; i++)
                    {
                        System.Numerics.Vector2 direction =
                            baseDirection;


                        if (weapon.ProjectileSpread > 0 &&
                            projectileCount > 1)
                        {
                            float offset =
                                i - (projectileCount - 1) / 2.0f;


                            float angleDegrees =
                                offset * weapon.ProjectileSpread;


                            float angleRadians =
                                angleDegrees *
                                (MathF.PI / 180.0f);


                            float cos =
                                MathF.Cos(angleRadians);


                            float sin =
                                MathF.Sin(angleRadians);


                            direction =
                                new System.Numerics.Vector2(
                                    direction.X * cos -
                                    direction.Y * sin,

                                    direction.X * sin +
                                    direction.Y * cos);
                        }


                        Projectile projectile =
                            new Projectile(
                                attacker,
                                new DamageInfo(
                                    weapon.Damage,
                                    weapon.Name,
                                    weapon.DamageType,
                                    false));


                        projectile.Position =
                            attacker.Position;


                        projectile.Velocity =
                            direction * weapon.ProjectileSpeed;


                        projectile.Lifetime =
                            weapon.ProjectileLifetime;


                        projectile.Radius =
                            weapon.ProjectileRadius;


                        projectile.PierceRemaining =
                            weapon.ProjectilePierce;


                        world.AddEntity(projectile);
                    }

                    break;
            }


            weapon.StartCooldown(
                attacker.Stats.AttackSpeed);
        }


        if (attacker is Player)
        {
            DebugPlayerEvents = events.Count;
        }


        // NUEVO DEBUG
        DebugWeaponEvents = events.Count;


        return events;
    }
}