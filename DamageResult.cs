namespace RogueTest.Core.Combat;

public class DamageResult
{
    public float BaseDamage { get; init; }

    public float FinalDamage { get; init; }

    public DamageType Type { get; init; }

    public bool IsCritical { get; init; }

    public bool TargetDied { get; init; }

    public string Source { get; init; } = "";
}