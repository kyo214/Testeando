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
    float delta)
    {
        List<GameEvent> events = new();


        foreach (var entity in world.Entities)
        {
            if (entity is not Enemy enemy)
                continue;


            if (!enemy.Active || !enemy.IsAlive)
            {
                enemy.AIState =
                    EnemyAIState.Dead;

                enemy.MoveDirection =
                    Vector2.Zero;

                continue;
            }


            Vector2 difference =
                player.Position - enemy.Position;


            float distanceSquared =
                difference.LengthSquared();


            float attackRange =
                enemy.AttackRange;


            float detectionRange =
                enemy.DetectionRange;



            // =========================
            // FUERA DE DETECTION RANGE
            // =========================

            if (distanceSquared >
                detectionRange * detectionRange)
            {
                enemy.AIState =
                    EnemyAIState.Idle;

                enemy.MoveDirection =
                    Vector2.Zero;

                continue;
            }



            // =========================
            // DENTRO DE ATTACK RANGE
            // =========================

            if (distanceSquared <=
                attackRange * attackRange)
            {
                enemy.AIState =
                    EnemyAIState.Attack;


                enemy.MoveDirection =
                    Vector2.Zero;



                DamageResult? result =
                    enemy.UpdateAttack(
                        delta,
                        player);



                if (result != null)
                {
                    events.Add(
                        new DamageEvent(
                            enemy,
                            player,
                            result));



                    if (result.TargetDied)
                    {
                        events.Add(
                            new DeathEvent(
                                player));
                    }
                }


                continue;
            }



            // =========================
            // DETECTADO, PERO LEJOS
            // =========================

            enemy.AIState =
                EnemyAIState.Chase;


            if (distanceSquared > 0)
            {
                enemy.MoveDirection =
                    Vector2.Normalize(
                        difference);
            }
            else
            {
                enemy.MoveDirection =
                    Vector2.Zero;
            }
        }


        return events;
    }
}