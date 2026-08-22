namespace RogueTest.Core.Stats;

public class ExperienceComponent
{
    public int Current { get; private set; }

    public int Level { get; private set; } = 1;

    public int ExperienceToNextLevel { get; private set; } = 100;


    // DEBUG
    public string DebugInfo { get; private set; } = "";



    public int Add(int amount)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== EXPERIENCE ADD START ==========\n";


        DebugInfo +=
            $"BEFORE Add Current={Current} " +
            $"Level={Level} " +
            $"Next={ExperienceToNextLevel}\n";


        DebugInfo +=
            $"INPUT Amount={amount}\n";



        if (amount <= 0)
        {
            DebugInfo +=
                "BLOCKED Amount <= 0\n";


            DebugInfo +=
                "========== EXPERIENCE ADD END ==========\n";


            return 0;
        }



        DebugInfo +=
            "BEFORE Add Experience\n";


        int levelsGained = 0;


        Current += amount;


        DebugInfo +=
            $"AFTER Add Current={Current}\n";



        DebugInfo +=
            "BEFORE Level Check\n";


        while (Current >= ExperienceToNextLevel)
        {
            DebugInfo +=
                $"LEVEL CONDITION TRUE Current={Current} " +
                $"Required={ExperienceToNextLevel}\n";


            DebugInfo +=
                "BEFORE Consume Experience\n";


            Current -= ExperienceToNextLevel;


            DebugInfo +=
                $"AFTER Consume Remaining XP={Current}\n";



            DebugInfo +=
                "BEFORE Increase Level\n";


            Level++;


            levelsGained++;


            DebugInfo +=
                $"AFTER Level Increase Level={Level}\n";



            DebugInfo +=
                "BEFORE Calculate Next Level XP\n";


            ExperienceToNextLevel =
                Level * 100;


            DebugInfo +=
                $"AFTER Next Level XP={ExperienceToNextLevel}\n";
        }



        DebugInfo +=
            "AFTER Level Check\n";


        DebugInfo +=
            $"RESULT Levels Gained={levelsGained}\n";


        DebugInfo +=
            $"FINAL Current={Current} " +
            $"Level={Level} " +
            $"Next={ExperienceToNextLevel}\n";


        DebugInfo +=
            "========== EXPERIENCE ADD END ==========\n";


        return levelsGained;
    }
}