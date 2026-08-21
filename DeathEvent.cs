using RogueTest.Core.Entities;

namespace RogueTest.Core.Events;

public class DeathEvent : GameEvent
{
    public CharacterEntity Entity { get; }

    public DeathEvent(CharacterEntity entity)
    {
        Entity = entity;
    }
}