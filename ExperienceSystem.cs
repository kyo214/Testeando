using RogueTest.Core.Entities;
using RogueTest.Core.Events;

namespace RogueTest.Core.Systems;

public class ExperienceSystem
{
    public List<GameEvent> Process(
        Player player,
        List<GameEvent> events)
    {
        List<GameEvent> result =
            new();

        foreach (GameEvent gameEvent in events)
        {
            if (gameEvent is not DeathEvent deathEvent)
                continue;

            if (deathEvent.Entity is not Enemy enemy)
                continue;

            int oldLevel =
                player.Experience.Level;

            int levelsGained =
                player.Experience.Add(
                    enemy.ExperienceReward);

            for (int i = 1; i <= levelsGained; i++)
            {
                result.Add(
                    new LevelUpEvent(
                        player,
                        oldLevel + i));
            }
        }

        return result;
    }
}