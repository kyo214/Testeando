using RogueTest.Core.Entities;

namespace RogueTest.Core.World;

public class GameWorld
{
    private readonly List<Entity> _entities = new();

    public IReadOnlyList<Entity> Entities => _entities;

    public void AddEntity(Entity entity)
    {
        if (!_entities.Contains(entity))
        {
            _entities.Add(entity);
        }
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
    }

    public void Clear()
    {
        _entities.Clear();
    }

    public void Update(float delta)
    {
        foreach (Entity entity in _entities)
        {
            if (entity.Active)
            {
                entity.Update(delta);
            }
        }
    }
}