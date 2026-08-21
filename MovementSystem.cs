using RogueTest.Core.Entities;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class MovementSystem
{
    public void Update(GameWorld world, float delta)
    {
        foreach (var entity in world.Entities)
        {
            if (!entity.Active)
                continue;

            if (entity is not CharacterEntity character)
                continue;

            entity.Velocity =
                entity.MoveDirection * character.Stats.MoveSpeed;

            entity.Position += entity.Velocity * delta;
        }
    }
}