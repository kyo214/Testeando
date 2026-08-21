using RogueTest.Core.Weapons;

namespace RogueTest.Core.Entities;

public class EnemyDefinition
{
    public string Name { get; set; } = "Enemy";

    public float MaxHealth { get; set; } = 100;

    public float MoveSpeed { get; set; } = 100;

    public float Damage { get; set; } = 10;

    public float Defense { get; set; } = 0;

    public float AttackSpeed { get; set; } = 1;

    public float DetectionRange { get; set; } = 300;

    public float AttackRange { get; set; } = 50;

    public int ExperienceReward { get; set; } = 10;
    public WeaponDefinition? Weapon { get; set; }
}