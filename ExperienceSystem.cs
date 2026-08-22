using RogueTest.Core.Entities;
using RogueTest.Core.Events;

namespace RogueTest.Core.Systems;

public class ExperienceSystem
{
    public string DebugInfo { get; private set; } = "";



    public List<GameEvent> Process(
        Player player,
        List<GameEvent> events)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== EXPERIENCE SYSTEM START ==========\n";


        DebugInfo +=
            $"BEFORE Process Events={events.Count}\n";


        DebugInfo +=
            $"Player Level={player.Experience.Level}\n";


        DebugInfo +=
            $"Player XP={player.Experience.Current}\n";



        DebugInfo +=
            "BEFORE Create Result List\n";


        List<GameEvent> result =
            new();


        DebugInfo +=
            "AFTER Create Result List\n";



        foreach (GameEvent gameEvent in events)
        {
            DebugInfo +=
                "---------- PROCESS EVENT ----------\n";


            DebugInfo +=
                $"EVENT TYPE={gameEvent.GetType().Name}\n";



            DebugInfo +=
                "BEFORE DeathEvent Validation\n";


            if (gameEvent is not DeathEvent deathEvent)
            {
                DebugInfo +=
                    "AFTER DeathEvent Validation FAILED\n";


                DebugInfo +=
                    "SKIP Not DeathEvent\n";


                continue;
            }


            DebugInfo +=
                "AFTER DeathEvent Validation OK\n";



            DebugInfo +=
                "BEFORE Enemy Validation\n";


            if (deathEvent.Entity is not Enemy enemy)
            {
                DebugInfo +=
                    "AFTER Enemy Validation FAILED\n";


                DebugInfo +=
                    "SKIP Death Entity not Enemy\n";


                continue;
            }


            DebugInfo +=
                "AFTER Enemy Validation OK\n";



            DebugInfo +=
                $"ENEMY DEAD={enemy.Name}\n";


            DebugInfo +=
                $"XP REWARD={enemy.ExperienceReward}\n";



            int oldLevel =
                player.Experience.Level;


            int oldXP =
                player.Experience.Current;



            DebugInfo +=
                "BEFORE Experience.Add\n";


            int levelsGained =
                player.Experience.Add(
                    enemy.ExperienceReward);



            DebugInfo +=
                "AFTER Experience.Add\n";



            DebugInfo +=
                $"XP {oldXP} -> {player.Experience.Current}\n";


            DebugInfo +=
                $"LEVEL {oldLevel} -> {player.Experience.Level}\n";


            DebugInfo +=
                $"LEVELS GAINED={levelsGained}\n";



            DebugInfo +=
                "BEFORE Create LevelUp Events\n";


            for (int i = 1; i <= levelsGained; i++)
            {
                int newLevel =
                    oldLevel + i;


                DebugInfo +=
                    $"CREATE LevelUpEvent Level={newLevel}\n";


                result.Add(
                    new LevelUpEvent(
                        player,
                        newLevel));


                DebugInfo +=
                    $"AFTER Add LevelUpEvent Count={result.Count}\n";
            }


            DebugInfo +=
                "AFTER Create LevelUp Events\n";
        }



        DebugInfo +=
            $"RETURN Events={result.Count}\n";


        DebugInfo +=
            "========== EXPERIENCE SYSTEM END ==========\n";


        return result;
    }
}