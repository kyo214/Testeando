using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class CollisionSystem
{
    public List<GameEvent> Update(
    GameWorld world,
    CombatSystem combat)
    {
        List<GameEvent> events = new();

        foreach (var entity in world.Entities)
        {
            if (entity is not Projectile projectile)
                continue;

            if (!projectile.Active)
                continue;

            foreach (var targetEntity in world.Entities)
            {
                if (targetEntity is not CharacterEntity target)
                    continue;

                if (!target.Active || !target.IsAlive)
                    continue;

                // No golpearse a sí mismo
                if (target == projectile.Owner)
                    continue;

                // No volver a golpear al mismo objetivo
                if (projectile.HitTargets.Contains(target))
                    continue;

                // Player -> Enemy
                if (projectile.Owner is Player &&
                    target is not Enemy)
                    continue;

                // Enemy -> Player
                if (projectile.Owner is Enemy &&
                    target is not Player)
                    continue;

                float distance =
                    System.Numerics.Vector2.Distance(
                        projectile.Position,
                        target.Position);

                if (distance > projectile.Radius)
                    continue;

                DamageResult result =
                    combat.Attack(
                        projectile.Owner,
                        target,
                        projectile.Damage);

                events.Add(
                    new DamageEvent(
                        projectile.Owner,
                        target,
                        result));

                // Registrar objetivo golpeado
                projectile.HitTargets.Add(target);

                if (result.TargetDied)
                {
                    events.Add(
                        new DeathEvent(target));
                }

                // ¿Puede atravesar otro objetivo?
                if (projectile.PierceRemaining > 0)
                {
                    projectile.PierceRemaining--;
                }
                else
                {
                    projectile.Active = false;
                }

                break;
            }
        }

        return events;
    }
}