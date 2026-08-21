using System.Numerics;

namespace RogueTest.Core.Entities;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();

    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public Vector2 MoveDirection { get; set; }

    public float Rotation { get; set; }

    public bool Active { get; set; } = true;
    public string Name { get; set; } = "";
    public virtual void Update(float delta)
    {
    }
}