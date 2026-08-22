using System.Numerics;
using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Weapons;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class EnemySpawnSystem
{
    public string DebugInfo { get; private set; } = "";


    public Enemy Spawn(
        GameWorld world,
        EnemyDefinition definition,
        Vector2 position,
        CombatSystem combat)
    {
        DebugInfo = "";

        DebugInfo +=
            "========== ENEMY SPAWN START ==========\n";


        DebugInfo +=
            $"Definition Name={definition.Name}\n";

        DebugInfo +=
            $"Position={position}\n";



        // =========================
        // CREATE ENTITY
        // =========================

        DebugInfo +=
            "BEFORE Create Enemy\n";


        Enemy enemy = new Enemy();


        DebugInfo +=
            "AFTER Create Enemy\n";



        // =========================
        // BASIC REFERENCES
        // =========================

        enemy.Combat = combat;

        enemy.Name = definition.Name;

        enemy.Position = position;


        DebugInfo +=
            $"Enemy Created Name={enemy.Name}\n";

        DebugInfo +=
            $"Combat Assigned={enemy.Combat != null}\n";



        // =========================
        // STATS
        // =========================

        DebugInfo +=
            "BEFORE Apply Stats\n";


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


        DebugInfo +=
            $"HP={enemy.Stats.Health}/{enemy.Stats.MaxHealth}\n";

        DebugInfo +=
            $"Damage={enemy.Stats.Damage}\n";

        DebugInfo +=
            $"Defense={enemy.Stats.Defense}\n";

        DebugInfo +=
            $"DetectionRange={enemy.DetectionRange}\n";

        DebugInfo +=
            $"AttackRange={enemy.AttackRange}\n";



        // =========================
        // WEAPON
        // =========================

        if (definition.Weapon != null)
        {
            DebugInfo +=
                "Weapon Definition Found\n";


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


            DebugInfo +=
                $"Weapon Created={weapon.Name}\n";


            enemy.Weapons.Add(weapon);


            DebugInfo +=
                $"Enemy Weapons Count={enemy.Weapons.Count}\n";


            enemy.Weapon = weapon;


            DebugInfo +=
                "Enemy Weapon Assigned\n";
        }
        else
        {
            DebugInfo +=
                "No Weapon Definition\n";
        }



        // =========================
        // ADD WORLD
        // =========================

        DebugInfo +=
            $"World Entities BEFORE Add={world.Entities.Count}\n";


        DebugInfo +=
            "BEFORE World.AddEntity\n";


        world.AddEntity(enemy);


        DebugInfo +=
            "AFTER World.AddEntity\n";


        DebugInfo +=
            $"World Entities AFTER Add={world.Entities.Count}\n";



        DebugInfo +=
            "========== ENEMY SPAWN END ==========\n";


        return enemy;
    }
}