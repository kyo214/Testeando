using RogueTest.Core.Combat;
using RogueTest.Core.Events;
using RogueTest.Core.Stats;
using RogueTest.Core.Weapons;

namespace RogueTest.Core.Entities;

public class Player : CharacterEntity
{
    public ExperienceComponent Experience { get; } = new();
    public int DebugAttackCalls { get; private set; }
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

    public List<GameEvent> Attack(Enemy target)
    {
        List<GameEvent> events = new();

        if (target == null)
            return events;

        if (Weapon == null)
            return events;

        if (Combat == null)
            return events;


        DamageInfo damage =
            Combat.CreateDamage(
                this,
                Weapon);


        DamageResult result =
            Combat.Attack(
                this,
                target,
                damage);


        events.Add(
            new DamageEvent(
                this,
                target,
                result));


        if (result.TargetDied)
        {
            events.Add(
                new DeathEvent(target));
        }


        return events;
    }
}