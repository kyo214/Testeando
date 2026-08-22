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
        if (Combat != null)
        {
            Combat.DebugTag +=
                "========== PLAYER DAMAGE ==========\n";

            Combat.DebugTag +=
                $"BEFORE HP={Stats.Health}\n";
        }


        Stats.TakeDamage(damage);


        if (Combat != null)
        {
            Combat.DebugTag +=
                $"AFTER HP={Stats.Health}\n";

            Combat.DebugTag +=
                "========== END PLAYER DAMAGE ==========\n";
        }
    }



    public void Heal(float amount)
    {
        if (Combat != null)
        {
            Combat.DebugTag +=
                $"PLAYER HEAL Amount={amount}\n";
        }


        Stats.Heal(amount);
    }





    public List<GameEvent> Attack(Enemy target)
    {
        List<GameEvent> events =
            new();



        if (Combat != null)
        {
            Combat.DebugTag +=
                "========== PLAYER ATTACK START ==========\n";
        }



        if (target == null)
        {
            if (Combat != null)
            {
                Combat.DebugTag +=
                    "BLOCKED Target null\n";
            }

            return events;
        }


        if (Combat != null)
        {
            Combat.DebugTag +=
                $"TARGET={target.Name}\n";
        }




        if (Weapon == null)
        {
            if (Combat != null)
            {
                Combat.DebugTag +=
                    "BLOCKED Weapon null\n";
            }

            return events;
        }



        if (Combat == null)
        {
            return events;
        }



        Combat.DebugTag +=
            $"Weapon={Weapon.Name}\n";



        Combat.DebugTag +=
            "BEFORE CreateDamage\n";



        DamageInfo damage =
            Combat.CreateDamage(
                this,
                Weapon);



        Combat.DebugTag +=
            $"AFTER CreateDamage Amount={damage.Amount}\n";



        Combat.DebugTag +=
            "BEFORE Combat.Attack\n";



        DamageResult result =
            Combat.Attack(
                this,
                target,
                damage);



        Combat.DebugTag +=
            "AFTER Combat.Attack\n";



        Combat.DebugTag +=
            $"RESULT Damage={result.FinalDamage}\n";


        Combat.DebugTag +=
            $"RESULT TargetDied={result.TargetDied}\n";



        Combat.DebugTag +=
            "BEFORE Add DamageEvent\n";



        events.Add(
            new DamageEvent(
                this,
                target,
                result));



        Combat.DebugTag +=
            $"AFTER Add DamageEvent Count={events.Count}\n";



        if (result.TargetDied)
        {
            Combat.DebugTag +=
                "BEFORE Add DeathEvent\n";


            events.Add(
                new DeathEvent(target));


            Combat.DebugTag +=
                $"AFTER Add DeathEvent Count={events.Count}\n";
        }



        Combat.DebugTag +=
            $"RETURN Player Attack Events={events.Count}\n";


        Combat.DebugTag +=
            "========== PLAYER ATTACK END ==========\n";



        return events;
    }
}