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
        List<GameEvent> events =
            new();


        combat.DebugTag +=
            "========== COLLISION START ==========\n";


        combat.DebugTag +=
            $"BEFORE Scan Entities Count={world.Entities.Count}\n";



        foreach (var entity in world.Entities)
        {
            combat.DebugTag +=
                $"CHECK Entity={entity.GetType().Name}\n";


            if (entity is not Projectile projectile)
            {
                continue;
            }


            combat.DebugTag +=
                $"AFTER Projectile Found Owner={projectile.Owner.GetType().Name}\n";



            if (!projectile.Active)
            {
                combat.DebugTag +=
                    "BLOCKED Projectile inactive\n";

                continue;
            }


            combat.DebugTag +=
                "AFTER Projectile Validation\n";



            combat.DebugTag +=
                "BEFORE Search Targets\n";



            foreach (var targetEntity in world.Entities)
            {
                if (targetEntity is not CharacterEntity target)
                {
                    continue;
                }


                combat.DebugTag +=
                    $"CHECK Target={target.GetType().Name}\n";



                if (!target.Active || !target.IsAlive)
                {
                    combat.DebugTag +=
                        "BLOCKED Target inactive/dead\n";

                    continue;
                }



                if (target == projectile.Owner)
                {
                    combat.DebugTag +=
                        "BLOCKED Same Owner\n";

                    continue;
                }



                if (projectile.HitTargets.Contains(target))
                {
                    combat.DebugTag +=
                        "BLOCKED Already Hit\n";

                    continue;
                }



                if (projectile.Owner is Player &&
                    target is not Enemy)
                {
                    combat.DebugTag +=
                        "BLOCKED Player Invalid Target\n";

                    continue;
                }



                if (projectile.Owner is Enemy &&
                    target is not Player)
                {
                    combat.DebugTag +=
                        "BLOCKED Enemy Invalid Target\n";

                    continue;
                }



                combat.DebugTag +=
                    "BEFORE Distance Check\n";


                float distance =
                    System.Numerics.Vector2.Distance(
                        projectile.Position,
                        target.Position);



                combat.DebugTag +=
                    $"Distance={distance} Radius={projectile.Radius}\n";



                if (distance > projectile.Radius)
                {
                    combat.DebugTag +=
                        "NO COLLISION\n";

                    continue;
                }



                combat.DebugTag +=
                    $"COLLISION HIT Target={target.Name}\n";



                combat.DebugTag +=
                    "BEFORE Combat.Attack\n";



                DamageResult result =
                    combat.Attack(
                        projectile.Owner,
                        target,
                        projectile.Damage);



                combat.DebugTag +=
                    "AFTER Combat.Attack\n";



                combat.DebugTag +=
                    "BEFORE Add DamageEvent\n";


                events.Add(
                    new DamageEvent(
                        projectile.Owner,
                        target,
                        result));


                combat.DebugTag +=
                    $"AFTER Add DamageEvent Count={events.Count}\n";



                projectile.HitTargets.Add(target);


                combat.DebugTag +=
                    "AFTER Register Hit Target\n";



                if (result.TargetDied)
                {
                    combat.DebugTag +=
                        "BEFORE Add DeathEvent\n";


                    events.Add(
                        new DeathEvent(target));


                    combat.DebugTag +=
                        $"AFTER Add DeathEvent Count={events.Count}\n";
                }



                combat.DebugTag +=
                    "BEFORE Pierce Check\n";


                if (projectile.PierceRemaining > 0)
                {
                    projectile.PierceRemaining--;


                    combat.DebugTag +=
                        $"AFTER Pierce Remaining={projectile.PierceRemaining}\n";
                }
                else
                {
                    projectile.Active = false;


                    combat.DebugTag +=
                        "AFTER Projectile Destroyed\n";
                }



                break;
            }
        }



        combat.DebugTag +=
            $"COLLISION RETURN Events={events.Count}\n";


        combat.DebugTag +=
            "========== COLLISION END ==========\n";


        return events;
    }
}