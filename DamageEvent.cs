using RogueTest.Core.Combat;
using RogueTest.Core.Entities;

namespace RogueTest.Core.Events;

public class DamageEvent : GameEvent
{
    public CharacterEntity Attacker { get; }

    public CharacterEntity Target { get; }

    public DamageResult Result { get; }

    public DamageEvent(
        CharacterEntity attacker,
        CharacterEntity target,
        DamageResult result)
    {
        Attacker = attacker;
        Target = target;
        Result = result;
    }
}