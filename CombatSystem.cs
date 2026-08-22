using RogueTest.Core.Entities;
using RogueTest.Core.Weapons;

namespace RogueTest.Core.Combat;

public class CombatSystem
{
    private readonly Random _random = new();


    public int DebugAttackCalls { get; private set; }

    public string DebugLastAttacker { get; private set; } = "";

    public string DebugLastTarget { get; private set; } = "";

    public string DebugLastSource { get; private set; } = "";

    public string DebugOrigin { get; set; } = "";

    public string DebugTag { get; set; } = "";

    public string DebugCombat { get; private set; } = "";

    public string DebugLastCaller { get; private set; } = "";



    public DamageResult Attack(
        CharacterEntity attacker,
        CharacterEntity target,
        DamageInfo damage)
    {
        DebugTag = "";


        DebugTag +=
            "========== COMBAT ATTACK START ==========\n";


        DebugTag +=
            "BEFORE StackTrace\n";


        var frame =
            new System.Diagnostics.StackTrace()
                .GetFrame(1)
                .GetMethod();


        DebugLastCaller =
            $"{frame.DeclaringType?.Name}.{frame.Name}";


        DebugTag +=
            $"AFTER StackTrace Caller={DebugLastCaller}\n";



        DebugTag +=
            "BEFORE Combat Info\n";


        DebugTag +=
            $"Source: {damage.Source}\n";

        DebugTag +=
            $"Attacker: {attacker.GetType().Name}\n";

        DebugTag +=
            $"Target: {target.GetType().Name}\n";


        DebugTag +=
            "AFTER Combat Info\n";



        DebugLastSource =
            damage.Source;


        DebugAttackCalls++;


        DebugLastAttacker =
            attacker.GetType().Name;


        DebugLastTarget =
            target.GetType().Name;



        DebugTag +=
            "BEFORE Entity Validation\n";


        if (!attacker.Active || !target.Active)
        {
            DebugTag +=
                "BLOCKED: Entity inactive\n";

            return new DamageResult
            {
                BaseDamage = damage.Amount,
                FinalDamage = 0,
                Type = damage.Type,
                IsCritical = damage.IsCritical,
                TargetDied = false,
                Source = damage.Source
            };
        }


        DebugTag +=
            "AFTER Active Validation\n";



        if (!attacker.IsAlive || !target.IsAlive)
        {
            DebugTag +=
                "BLOCKED: Entity dead\n";

            return new DamageResult
            {
                BaseDamage = damage.Amount,
                FinalDamage = 0,
                Type = damage.Type,
                IsCritical = damage.IsCritical,
                TargetDied = false,
                Source = damage.Source
            };
        }


        DebugTag +=
            "AFTER Alive Validation\n";



        DebugTag +=
            "BEFORE Base Damage\n";


        float finalDamage =
            damage.Amount;


        DebugTag +=
            $"Base Damage={finalDamage}\n";


        DebugTag +=
            "AFTER Base Damage\n";



        if (damage.IsCritical)
        {
            DebugTag +=
                "BEFORE Critical\n";


            finalDamage *=
                attacker.Stats.CriticalMultiplier;


            DebugTag +=
                $"AFTER Critical Damage={finalDamage}\n";
        }



        DebugTag +=
            "BEFORE Defense\n";


        float defense =
            target.Stats.Defense;


        finalDamage -=
            defense;


        DebugTag +=
            $"AFTER Defense ({defense}) Damage={finalDamage}\n";


        if (finalDamage < 0)
        {
            finalDamage = 0;

            DebugTag +=
                "Damage clamped to 0\n";
        }



        DebugTag +=
            "BEFORE Resistance\n";


        float resistance =
            target.Stats.Resistances
                .GetResistance(damage.Type);


        finalDamage *=
            1.0f - resistance;


        DebugTag +=
            $"AFTER Resistance ({resistance}) Damage={finalDamage}\n";


        if (finalDamage < 0)
        {
            finalDamage = 0;

            DebugTag +=
                "Damage clamped after resistance\n";
        }



        DebugTag +=
            "BEFORE Apply Damage\n";


        float hpBefore =
            target.Stats.Health;


        target.Stats.TakeDamage(finalDamage);


        float hpAfter =
            target.Stats.Health;


        DebugTag +=
            $"AFTER Apply Damage HP {hpBefore}->{hpAfter}\n";



        if (!target.IsAlive)
        {
            DebugTag +=
                "BEFORE Death State\n";


            target.Active = false;


            DebugTag +=
                "AFTER Death State TARGET DIED\n";
        }



        DebugTag +=
            "========== COMBAT ATTACK END ==========\n";


        return new DamageResult
        {
            BaseDamage = damage.Amount,
            FinalDamage = finalDamage,
            Type = damage.Type,
            IsCritical = damage.IsCritical,
            TargetDied = !target.IsAlive,
            Source = damage.Source
        };
    }





    public DamageInfo CreateDamage(
        CharacterEntity attacker,
        Weapon weapon)
    {
        DebugTag +=
            "========== CREATE DAMAGE START ==========\n";


        DebugTag +=
            $"Weapon={weapon.Name}\n";


        DebugTag +=
            $"Base Player Damage={attacker.Stats.Damage}\n";


        DebugTag +=
            $"Weapon Damage={weapon.Damage}\n";


        DebugTag +=
            "BEFORE Critical Roll\n";


        bool critical =
            _random.NextDouble() <
            attacker.Stats.CriticalChance;


        DebugTag +=
            $"AFTER Critical Roll Result={critical}\n";


        DamageInfo result =
            new DamageInfo(
                attacker.Stats.Damage + weapon.Damage,
                weapon.Name,
                weapon.DamageType,
                critical);


        DebugTag +=
            $"CREATE DAMAGE RESULT Amount={result.Amount}\n";


        DebugTag +=
            "========== CREATE DAMAGE END ==========\n";


        return result;
    }
}