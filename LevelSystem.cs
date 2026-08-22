using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Progression;

namespace RogueTest.Core.Systems;

public class LevelSystem
{
    private readonly LevelProgression progression =
        new();


    public string DebugInfo { get; private set; } = "";



    public void Process(
        Player player,
        List<GameEvent> events)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== LEVEL SYSTEM START ==========\n";


        DebugInfo +=
            $"BEFORE Process Events={events.Count}\n";


        DebugInfo +=
            $"Player Current Level={player.Experience.Level}\n";



        foreach (GameEvent gameEvent in events)
        {
            DebugInfo +=
                "---------- PROCESS EVENT ----------\n";


            DebugInfo +=
                $"EVENT TYPE={gameEvent.GetType().Name}\n";



            DebugInfo +=
                "BEFORE LevelUpEvent Validation\n";


            if (gameEvent is not LevelUpEvent levelUpEvent)
            {
                DebugInfo +=
                    "SKIP Not LevelUpEvent\n";

                continue;
            }


            DebugInfo +=
                "AFTER LevelUpEvent Validation OK\n";



            DebugInfo +=
                "BEFORE Player Validation\n";


            if (levelUpEvent.Player != player)
            {
                DebugInfo +=
                    "SKIP Different Player\n";

                continue;
            }


            DebugInfo +=
                "AFTER Player Validation OK\n";



            int level =
                levelUpEvent.NewLevel;


            DebugInfo +=
                $"LEVEL UP APPLY Level={level}\n";



            // =========================
            // MAX HEALTH
            // =========================

            DebugInfo +=
                "BEFORE MaxHealth Bonus\n";


            float healthBonus =
                progression.GetMaxHealthBonus(level);


            DebugInfo +=
                $"BONUS MaxHealth={healthBonus}\n";


            player.Stats.MaxHealth +=
                healthBonus;


            DebugInfo +=
                $"AFTER MaxHealth={player.Stats.MaxHealth}\n";



            // =========================
            // DAMAGE
            // =========================

            DebugInfo +=
                "BEFORE Damage Bonus\n";


            float damageBonus =
                progression.GetDamageBonus(level);


            DebugInfo +=
                $"BONUS Damage={damageBonus}\n";


            player.Stats.Damage +=
                damageBonus;


            DebugInfo +=
                $"AFTER Damage={player.Stats.Damage}\n";



            // =========================
            // DEFENSE
            // =========================

            DebugInfo +=
                "BEFORE Defense Bonus\n";


            float defenseBonus =
                progression.GetDefenseBonus(level);


            DebugInfo +=
                $"BONUS Defense={defenseBonus}\n";


            player.Stats.Defense +=
                defenseBonus;


            DebugInfo +=
                $"AFTER Defense={player.Stats.Defense}\n";



            DebugInfo +=
                "LEVEL UP COMPLETE\n";
        }



        DebugInfo +=
            $"FINAL Player Level={player.Experience.Level}\n";


        DebugInfo +=
            "========== LEVEL SYSTEM END ==========\n";
    }
}