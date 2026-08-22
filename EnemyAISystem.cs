using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.World;
using System.Numerics;

namespace RogueTest.Core.Systems;

public class EnemyAISystem
{
    public List<GameEvent> Update(
    GameWorld world,
    Player player,
    float delta,
    float restartProtectionTimer)
    {
        List<GameEvent> events =
            new();


        CombatSystem? combat = null;


        foreach (var entity in world.Entities)
        {
            if (entity is not Enemy enemy)
                continue;



            // DEBUG
            enemy.DebugInfo =
                "========== ENEMY AI UPDATE ==========\n";


            enemy.DebugInfo +=
                $"Enemy={enemy.Name}\n";



            if (!enemy.Active || !enemy.IsAlive)
            {
                enemy.DebugInfo +=
                    "BLOCKED Enemy Dead/Inactive\n";


                enemy.AIState =
                    EnemyAIState.Dead;


                enemy.MoveDirection =
                    Vector2.Zero;


                continue;
            }



            enemy.DebugInfo +=
                "AFTER Alive Validation\n";



            enemy.DebugInfo +=
                "BEFORE Calculate Direction\n";


            Vector2 difference =
                player.Position - enemy.Position;



            float distanceSquared =
                difference.LengthSquared();



            float attackRange =
                enemy.AttackRange;


            float detectionRange =
                enemy.DetectionRange;



            enemy.DebugInfo +=
                $"DistanceSquared={distanceSquared}\n";


            enemy.DebugInfo +=
                $"DetectionRange={detectionRange}\n";


            enemy.DebugInfo +=
                $"AttackRange={attackRange}\n";



            // =========================
            // FUERA DE DETECTION RANGE
            // =========================


            enemy.DebugInfo +=
                "BEFORE Detection Check\n";


            if (distanceSquared >
                detectionRange * detectionRange)
            {
                enemy.DebugInfo +=
                    "RESULT Outside Detection Range\n";


                enemy.AIState =
                    EnemyAIState.Idle;


                enemy.MoveDirection =
                    Vector2.Zero;


                continue;
            }


            enemy.DebugInfo +=
                "RESULT Player Detected\n";




            // =========================
            // DENTRO DE ATTACK RANGE
            // =========================


            enemy.DebugInfo +=
                "BEFORE Attack Range Check\n";


            if (distanceSquared <=
                attackRange * attackRange)
            {
                enemy.DebugInfo +=
                    "RESULT Inside Attack Range\n";


                enemy.AIState =
                    EnemyAIState.Attack;


                enemy.MoveDirection =
                    Vector2.Zero;



                enemy.DebugInfo +=
                    "BEFORE Enemy.UpdateAttack\n";


                // =========================
                // RESTART PROTECTION
                // =========================

                if (restartProtectionTimer > 0)
                {
                    enemy.DebugInfo +=
                        "ATTACK BLOCKED Restart Protection\n";

                    continue;
                }


                DamageResult? result =
                    enemy.UpdateAttack(
                        delta,
                        player);


                enemy.DebugInfo +=
                    "AFTER Enemy.UpdateAttack\n";



                if (result != null)
                {
                    enemy.DebugInfo +=
                        $"Attack Result Damage={result.FinalDamage}\n";


                    events.Add(
                        new DamageEvent(
                            enemy,
                            player,
                            result));


                    enemy.DebugInfo +=
                        $"ADD DamageEvent Count={events.Count}\n";



                    if (result.TargetDied)
                    {
                        events.Add(
                            new DeathEvent(
                                player));


                        enemy.DebugInfo +=
                            $"ADD DeathEvent Count={events.Count}\n";
                    }
                }
                else
                {
                    enemy.DebugInfo +=
                        "No Attack Result (Cooldown or State)\n";
                }



                continue;
            }



            // =========================
            // DETECTADO PERO LEJOS
            // =========================


            enemy.DebugInfo +=
                "RESULT Chase\n";


            enemy.AIState =
                EnemyAIState.Chase;



            enemy.DebugInfo +=
                "BEFORE Calculate Movement\n";



            if (distanceSquared > 0)
            {
                enemy.MoveDirection =
                    Vector2.Normalize(
                        difference);


                enemy.DebugInfo +=
                    $"MoveDirection={enemy.MoveDirection}\n";
            }
            else
            {
                enemy.MoveDirection =
                    Vector2.Zero;


                enemy.DebugInfo +=
                    "MoveDirection Zero\n";
            }


            enemy.DebugInfo +=
                "========== END ENEMY AI UPDATE ==========\n";
        }



        return events;
    }
}