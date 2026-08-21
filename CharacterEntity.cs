using RogueTest.Core.Stats;
using RogueTest.Core.Weapons;

namespace RogueTest.Core.Entities;

public abstract class CharacterEntity : Entity
{
    public CharacterStats Stats { get; } = new();
    public List<Weapon> Weapons { get; } = new();
    public bool IsAlive => Stats.IsAlive;

}