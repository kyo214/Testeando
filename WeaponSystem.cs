using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Weapons;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class WeaponSystem
{
    private readonly TargetingSystem targetingSystem =
        new TargetingSystem();


    public int DebugAttackCount { get; private set; }

    public int DebugEventCount { get; private set; }

    public int DebugPlayerAttackCount { get; private set; }

    public int DebugPlayerEventCount { get; private set; }

    public int DebugPlayerAttacks { get; private set; }

    public int DebugPlayerEvents { get; private set; }

    public int DebugWeaponEvents { get; private set; }



    public List<GameEvent> Update(
        CharacterEntity attacker,
        GameWorld world,
        CombatSystem combat,
        float delta)
    {

        combat.DebugTag +=
            "========== WEAPON SYSTEM START ==========\n";


        combat.DebugTag +=
            $"Attacker={attacker.GetType().Name}\n";



        if (attacker is Player)
        {
            DebugPlayerAttacks = 0;
            DebugPlayerEvents = 0;

            combat.DebugTag +=
                "RESET Player Debug Counters\n";
        }



        List<GameEvent> events =
            new();



        combat.DebugTag +=
            $"Weapons Count={attacker.Weapons.Count}\n";



        foreach (Weapon weapon in attacker.Weapons)
        {
            combat.DebugTag +=
                "---------- WEAPON LOOP ----------\n";


            combat.DebugTag +=
                $"Weapon={weapon.Name}\n";



            combat.DebugTag +=
                "BEFORE Weapon.Update\n";


            weapon.Update(delta);


            combat.DebugTag +=
                "AFTER Weapon.Update\n";



            if (attacker is Enemy enemy &&
                enemy.AIState != EnemyAIState.Attack)
            {
                combat.DebugTag +=
                    $"BLOCKED Enemy AI State={enemy.AIState}\n";

                continue;
            }



            combat.DebugTag +=
                "BEFORE CanAttack Check\n";


            if (!weapon.CanAttack())
            {
                combat.DebugTag +=
                    "BLOCKED Cooldown\n";

                continue;
            }


            combat.DebugTag +=
                "AFTER CanAttack OK\n";



            combat.DebugTag +=
                "BEFORE FindTarget\n";


            CharacterEntity? target =
                targetingSystem.FindTarget(
                    attacker,
                    world,
                    weapon.Range,
                    weapon.TargetingMode);



            if (target == null)
            {
                combat.DebugTag +=
                    "AFTER FindTarget NULL\n";

                continue;
            }



            combat.DebugTag +=
                $"AFTER FindTarget Target={target.Name}\n";



            if (attacker is Player)
            {
                DebugPlayerAttacks++;

                combat.DebugTag +=
                    $"Player Attack Count={DebugPlayerAttacks}\n";
            }



            combat.DebugTag +=
                $"ATTACK TYPE={weapon.AttackType}\n";



            switch (weapon.AttackType)
            {

                case WeaponAttackType.Direct:

                    combat.DebugTag +=
                        "ENTER DIRECT ATTACK\n";


                    combat.DebugTag +=
                        "BEFORE CreateDamage\n";


                    DamageInfo damage =
                        combat.CreateDamage(
                            attacker,
                            weapon);


                    combat.DebugTag +=
                        $"AFTER CreateDamage Amount={damage.Amount}\n";



                    combat.DebugTag +=
                        "BEFORE Combat.Attack\n";


                    DamageResult result =
                        combat.Attack(
                            attacker,
                            target,
                            damage);



                    combat.DebugTag +=
                        "AFTER Combat.Attack\n";



                    combat.DebugTag +=
                        "BEFORE Add DamageEvent\n";


                    events.Add(
                        new DamageEvent(
                            attacker,
                            target,
                            result));


                    combat.DebugTag +=
                        $"AFTER Add DamageEvent Count={events.Count}\n";



                    if (result.TargetDied)
                    {
                        combat.DebugTag +=
                            "BEFORE Add DeathEvent\n";


                        events.Add(
                            new DeathEvent(target));


                        combat.DebugTag +=
                            $"AFTER Add DeathEvent Count={events.Count}\n";
                    }


                    break;



                case WeaponAttackType.Projectile:

                    combat.DebugTag +=
                        "ENTER PROJECTILE ATTACK\n";


                    // Tu código de proyectiles va aquí


                    combat.DebugTag +=
                        "EXIT PROJECTILE ATTACK\n";


                    break;
            }



            combat.DebugTag +=
                "BEFORE StartCooldown\n";


            weapon.StartCooldown(
                attacker.Stats.AttackSpeed);


            combat.DebugTag +=
                "AFTER StartCooldown\n";
        }



        if (attacker is Player)
        {
            DebugPlayerEvents =
                events.Count;
        }



        DebugWeaponEvents =
            events.Count;



        combat.DebugTag +=
            $"RETURN WeaponSystem Events={events.Count}\n";


        combat.DebugTag +=
            "========== WEAPON SYSTEM END ==========\n";


        return events;
    }
}