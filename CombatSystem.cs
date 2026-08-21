using RogueTest.Core.Entities;
using RogueTest.Core.Weapons;

namespace RogueTest.Core.Combat;

public class CombatSystem
{
    private readonly Random _random = new();

    public DamageResult Attack(
    CharacterEntity attacker,
    CharacterEntity target,
    DamageInfo damage)
    {
        if (!attacker.Active || !target.Active)
            return new DamageResult
            {
                BaseDamage = damage.Amount,
                FinalDamage = 0,
                Type = damage.Type,
                IsCritical = damage.IsCritical,
                TargetDied = false,
                Source = damage.Source
            };

        if (!attacker.IsAlive || !target.IsAlive)
            return new DamageResult
            {
                BaseDamage = damage.Amount,
                FinalDamage = 0,
                Type = damage.Type,
                IsCritical = damage.IsCritical,
                TargetDied = false,
                Source = damage.Source
            };

        float finalDamage = damage.Amount;

        // Crítico
        if (damage.IsCritical)
        {
            finalDamage *= attacker.Stats.CriticalMultiplier;
        }

        // Defensa
        finalDamage -= target.Stats.Defense;

        if (finalDamage < 0)
            finalDamage = 0;

        // Resistencia
        float resistance =
            target.Stats.Resistances.GetResistance(damage.Type);

        finalDamage *= 1.0f - resistance;

        if (finalDamage < 0)
            finalDamage = 0;

        // Aplicar daño
        target.Stats.TakeDamage(finalDamage);

        if (!target.IsAlive)
        {
            target.Active = false;
        }

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
        bool critical =
            _random.NextDouble() <
            attacker.Stats.CriticalChance;

        return new DamageInfo(
            attacker.Stats.Damage + weapon.Damage,
            weapon.Name,
            weapon.DamageType,
            critical);
    }
}