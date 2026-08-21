using RogueTest.Core.Entities;

namespace RogueTest.Core.Events;

public class LevelUpEvent : GameEvent
{
    public Player Player { get; }

    public int NewLevel { get; }

    public LevelUpEvent(
        Player player,
        int newLevel)
    {
        Player = player;
        NewLevel = newLevel;
    }
}