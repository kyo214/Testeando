using System.Numerics;
using RogueTest.Core.Entities;
using RogueTest.Core.Weapons;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class EnemySpawnSystem
{
    public Enemy Spawn(
        GameWorld world,
        EnemyDefinition definition,
        Vector2 position)
    {
        Enemy enemy = new Enemy();

        enemy.Name = definition.Name;

        enemy.Position = position;

        enemy.Stats.MaxHealth =
            definition.MaxHealth;

        enemy.Stats.RestoreFullHealth();

        enemy.Stats.MoveSpeed =
            definition.MoveSpeed;

        enemy.Stats.Damage =
            definition.Damage;

        enemy.Stats.Defense =
            definition.Defense;

        enemy.Stats.AttackSpeed =
            definition.AttackSpeed;

        enemy.DetectionRange =
            definition.DetectionRange;

        enemy.AttackRange =
            definition.AttackRange;

        enemy.ExperienceReward =
            definition.ExperienceReward;

        if (definition.Weapon != null)
        {
            Weapon weapon = new Weapon
            {
                Name = definition.Weapon.Name,
                Damage = definition.Weapon.Damage,
                DamageType = definition.Weapon.DamageType,
                AttackCooldown = definition.Weapon.AttackCooldown,
                AttackType = definition.Weapon.AttackType,
                Range = definition.Weapon.Range,
                ProjectileSpeed = definition.Weapon.ProjectileSpeed,
                ProjectileLifetime = definition.Weapon.ProjectileLifetime,
                ProjectileRadius = definition.Weapon.ProjectileRadius,
                ProjectilePierce = definition.Weapon.ProjectilePierce,
                ProjectileCount = definition.Weapon.ProjectileCount,
                ProjectileSpread = definition.Weapon.ProjectileSpread,
                TargetingMode = definition.Weapon.TargetingMode
            };

            enemy.Weapons.Add(weapon);

            enemy.Weapon = weapon;
        }

        world.AddEntity(enemy);

        return enemy;
    }
}