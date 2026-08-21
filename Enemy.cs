using RogueTest.Core.Combat;
using RogueTest.Core.Systems;
using RogueTest.Core.Weapons;

namespace RogueTest.Core.Entities;

public class Enemy : CharacterEntity
{
    public float AttackRange { get; set; } = 50f;

    public float AttackCooldown { get; set; } = 1.0f;

    public EnemyAIState AIState { get; set; } =
        EnemyAIState.Idle;

    public bool CanAttack =>
        AIState == EnemyAIState.Attack;

    public float AttackCooldownRemaining { get; set; } = 0f;

    public Weapon? Weapon { get; set; }

    public CombatSystem? Combat { get; set; }

    public float DetectionRange { get; set; } = 300f;

    public int ExperienceReward { get; set; } = 10;


    public void TakeDamage(float damage)
    {
        Stats.TakeDamage(damage);
    }


    public DamageResult? Attack(CharacterEntity target)
    {
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
    public DamageResult? UpdateAttack(
    float delta,
    CharacterEntity target)
    {
        if (AttackCooldownRemaining > 0)
        {
            AttackCooldownRemaining -= delta;
            return null;
        }

        if (!CanAttack)
            return null;


        DamageResult? result =
            Attack(target);


        AttackCooldownRemaining =
            AttackCooldown;


        return result;
    }
}