namespace RogueTest.Core.Progression;

public class LevelProgression
{
    public int GetMaxHealthBonus(int level)
    {
        return 10;
    }

    public float GetDamageBonus(int level)
    {
        return 2;
    }

    public float GetDefenseBonus(int level)
    {
        return 1;
    }
}