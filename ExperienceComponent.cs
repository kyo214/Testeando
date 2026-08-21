namespace RogueTest.Core.Stats;

public class ExperienceComponent
{
    public int Current { get; private set; }
    public int Level { get; private set; } = 1;
    public int ExperienceToNextLevel { get; private set; } = 100;
    public int Add(int amount)
    {
        if (amount <= 0)
            return 0;

        int levelsGained = 0;

        Current += amount;

        while (Current >= ExperienceToNextLevel)
        {
            Current -= ExperienceToNextLevel;

            Level++;

            levelsGained++;

            ExperienceToNextLevel =
                Level * 100;
        }

        return levelsGained;
    }
}