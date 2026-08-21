using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Progression;

namespace RogueTest.Core.Systems;

public class LevelSystem
{
    private readonly LevelProgression progression =
        new();

    public void Process(
        Player player,
        List<GameEvent> events)
    {
        foreach (GameEvent gameEvent in events)
        {
            if (gameEvent is not LevelUpEvent levelUpEvent)
                continue;

            if (levelUpEvent.Player != player)
                continue;

            int level =
                levelUpEvent.NewLevel;

            // =========================
            // MAX HEALTH
            // =========================

            player.Stats.MaxHealth +=
                progression.GetMaxHealthBonus(level);

            // =========================
            // DAMAGE
            // =========================

            player.Stats.Damage +=
                progression.GetDamageBonus(level);

            // =========================
            // DEFENSE
            // =========================

            player.Stats.Defense +=
                progression.GetDefenseBonus(level);
        }
    }
}