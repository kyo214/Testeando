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


    // DEBUG
    public string DebugInfo { get;  set; } = "";



    public void TakeDamage(float damage)
    {
        DebugInfo +=
            "========== ENEMY DAMAGE ==========\n";


        DebugInfo +=
            $"BEFORE TakeDamage HP={Stats.Health}\n";


        Stats.TakeDamage(damage);


        DebugInfo +=
            $"AFTER TakeDamage HP={Stats.Health}\n";


        DebugInfo +=
            "========== END ENEMY DAMAGE ==========\n";
    }





    public DamageResult? Attack(CharacterEntity target)
    {
        DebugInfo =
            "========== ENEMY ATTACK START ==========\n";


        DebugInfo +=
            "BEFORE Weapon Validation\n";


        if (Weapon == null)
        {
            DebugInfo +=
                "CANCEL Weapon null\n";

            return null;
        }


        DebugInfo +=
            $"AFTER Weapon Validation Weapon={Weapon.Name}\n";



        DebugInfo +=
            "BEFORE Combat Validation\n";


        if (Combat == null)
        {
            DebugInfo +=
                "CANCEL Combat null\n";

            return null;
        }


        DebugInfo +=
            "AFTER Combat Validation\n";



        DebugInfo +=
            $"Target={target.GetType().Name}\n";


        DebugInfo +=
            "BEFORE CreateDamage\n";


        DamageInfo damage =
            Combat.CreateDamage(
                this,
                Weapon);


        DebugInfo +=
            $"AFTER CreateDamage Amount={damage.Amount}\n";



        DebugInfo +=
            "BEFORE Combat.Attack\n";


        DamageResult result =
            Combat.Attack(
                this,
                target,
                damage);


        DebugInfo +=
            $"AFTER Combat.Attack Damage={result.FinalDamage}\n";


        DebugInfo +=
            $"TargetDied={result.TargetDied}\n";


        DebugInfo +=
            "========== ENEMY ATTACK END ==========\n";


        return result;
    }






    public DamageResult? UpdateAttack(
        float delta,
        CharacterEntity target)
    {
        DebugInfo =
            "========== ENEMY UPDATE ATTACK ==========\n";


        DebugInfo +=
            $"Cooldown Before={AttackCooldownRemaining}\n";


        DebugInfo +=
            $"AI State={AIState}\n";



        if (AttackCooldownRemaining > 0)
        {
            AttackCooldownRemaining -= delta;


            DebugInfo +=
                $"COOLDOWN Remaining={AttackCooldownRemaining}\n";


            DebugInfo +=
                "EXIT UpdateAttack Cooldown\n";


            return null;
        }



        DebugInfo +=
            "AFTER Cooldown Validation\n";



        if (!CanAttack)
        {
            DebugInfo +=
                "EXIT Cannot Attack State\n";


            return null;
        }



        DebugInfo +=
            "BEFORE Execute Attack\n";


        DamageResult? result =
            Attack(target);



        DebugInfo +=
            "AFTER Execute Attack\n";


        AttackCooldownRemaining =
            AttackCooldown;


        DebugInfo +=
            $"Cooldown Reset={AttackCooldownRemaining}\n";


        DebugInfo +=
            "========== END UPDATE ATTACK ==========\n";


        return result;
    }
}