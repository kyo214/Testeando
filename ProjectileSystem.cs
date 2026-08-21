using RogueTest.Core.Entities;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class ProjectileSystem
{
    public void Update(
        GameWorld world,
        float delta)
    {
        foreach (var entity in world.Entities)
        {
            if (entity is not Projectile projectile)
                continue;

            if (!projectile.Active)
                continue;

            projectile.Position +=
                projectile.Velocity * delta;

            projectile.Lifetime -= delta;

            if (projectile.Lifetime <= 0)
            {
                projectile.Active = false;
            }
        }
    }
}