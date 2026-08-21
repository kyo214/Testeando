using RogueTest.Core.Entities;
using RogueTest.Core.World;
using System.Numerics;

namespace RogueTest.Core.Systems;

public class TargetingSystem
{
    public CharacterEntity? FindTarget(
    CharacterEntity attacker,
    GameWorld world,
    float range,
    TargetingMode targetingMode)
    {
        List<CharacterEntity> validTargets = new();

        float rangeSquared = range * range;

        foreach (var entity in world.Entities)
        {
            if (entity is not CharacterEntity target)
                continue;

            if (target == attacker)
                continue;

            if (!target.Active || !target.IsAlive)
                continue;

            // Player busca Enemy
            if (attacker is Player &&
                target is not Enemy)
                continue;

            // Enemy busca Player
            if (attacker is Enemy &&
                target is not Player)
                continue;

            Vector2 difference =
                target.Position - attacker.Position;

            float distanceSquared =
                difference.LengthSquared();

            if (distanceSquared > rangeSquared)
                continue;

            validTargets.Add(target);
        }

        if (validTargets.Count == 0)
            return null;

        switch (targetingMode)
        {
            case TargetingMode.Nearest:

                return validTargets
                    .OrderBy(target =>
                        Vector2.DistanceSquared(
                            attacker.Position,
                            target.Position))
                    .First();

            case TargetingMode.Farthest:

                return validTargets
                    .OrderByDescending(target =>
                        Vector2.DistanceSquared(
                            attacker.Position,
                            target.Position))
                    .First();

            case TargetingMode.LowestHealth:

                return validTargets
                    .OrderBy(target =>
                        target.Stats.Health)
                    .First();

            case TargetingMode.Random:

                return validTargets[
                    Random.Shared.Next(
                        validTargets.Count)];

            default:

                return validTargets[0];
        }
    }
}