namespace RogueTest.Core.Stats;

public class CharacterStats
{
    public float MaxHealth { get; set; } = 100;
    public float Health { get; private set; } = 100;

    public float MoveSpeed { get; set; } = 100;
    public float Damage { get; set; } = 10;
    public float AttackSpeed { get; set; } = 1;
    public float Defense { get; set; } = 0;

    public float CriticalChance { get; set; } = 0;

    public float CriticalMultiplier { get; set; } = 2;

    public DamageResistances Resistances { get; } = new();
    public bool IsAlive => Health > 0;
    public void TakeDamage(float damage)
    {
        if (damage < 0)
            return;

        Health -= damage;

        if (Health < 0)
            Health = 0;
    }

    public void Heal(float amount)
    {
        if (amount < 0)
            return;

        Health += amount;

        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public void RestoreFullHealth()
    {
        Health = MaxHealth;
    }
}
