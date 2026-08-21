namespace RogueTest.Core.Combat;

public class DamageInfo
{
    public float Amount { get; init; }

    public string Source { get; init; } = "";

    public DamageType Type { get; init; } = DamageType.Physical;

    public bool IsCritical { get; init; }

    public DamageInfo(
        float amount,
        string source,
        DamageType type = DamageType.Physical,
        bool isCritical = false)
    {
        Amount = amount;
        Source = source;
        Type = type;
        IsCritical = isCritical;
    }
}