using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class CleanupSystem
{
    public void Update(GameWorld world)
    {
        List<Entities.Entity> entitiesToRemove = new();

        foreach (var entity in world.Entities)
        {
            if (!entity.Active)
            {
                entitiesToRemove.Add(entity);
            }
        }

        foreach (var entity in entitiesToRemove)
        {
            world.RemoveEntity(entity);
        }
    }
}