using RogueTest.Core.Combat;

namespace RogueTest.Core.Stats;

public class DamageResistances
{
    private readonly Dictionary<DamageType, float> _resistances = new();

    public float GetResistance(DamageType type)
    {
        return _resistances.TryGetValue(type, out float resistance)
            ? resistance
            : 0;
    }

    public void SetResistance(DamageType type, float resistance)
    {
        _resistances[type] = resistance;
    }
}