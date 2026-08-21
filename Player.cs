using RogueTest.Core.Combat;
using RogueTest.Core.Stats;
using RogueTest.Core.Weapons;

namespace RogueTest.Core.Entities;

public class Player : CharacterEntity
{
    public ExperienceComponent Experience { get; } = new();

    public Weapon? Weapon { get; set; }
    public CombatSystem? Combat { get; set; }
    public void TakeDamage(float damage)
    {
        Stats.TakeDamage(damage);
    }

    public void Heal(float amount)
    {
        Stats.Heal(amount);
    }

    public DamageResult? Attack(Enemy target)
    {
        if (target == null)
            return null;

        if (Weapon == null)
            return null;

        if (Combat == null)
            return null;

        DamageInfo damage =
            Combat.CreateDamage(
                this,
                Weapon);

        return Combat.Attack(
            this,
            target,
            damage);
    }
}