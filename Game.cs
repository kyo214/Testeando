using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Systems;
using RogueTest.Core.World;
using System.Numerics;

namespace RogueTest.Core;

public class Game
{
    public Player Player { get; private set; }

    public GameWorld World { get; }

    public MovementSystem Movement { get; }

    public bool IsRunning { get; private set; }

    public CombatSystem Combat { get; }
    public WeaponSystem Weapons { get; }
    public List<DamageResult> LastDamageResults { get; private set; } = new();
    public List<GameEvent> LastEvents { get; private set; } = new();
    public CleanupSystem Cleanup { get; }
    public ProjectileSystem Projectiles { get; }
    public CollisionSystem Collision { get; }
    public EnemyAISystem EnemyAI { get; }
    public ExperienceSystem Experience { get; }
    public LevelSystem Level { get; }
    public EnemySpawnSystem EnemySpawn { get; }
    public WaveSystem Wave { get; }
    public MapSystem Map { get; }
    public IReadOnlyList<GameEvent> DebugEvents => LastEvents;
    public bool IsGameOver { get; private set; }
    private string _lastGameStateDebug = "";
    public string RestartDebug { get; private set; } = "";
    public float RestartProtectionTimer { get; private set; }
    public string RestartDebugTag = "";
    public int DebugPlayerWeapons =>
    Player.Weapons.Count;

    public int DebugWorldEntities =>
        World.Entities.Count;

    public bool DebugWorldHasPlayer =>
        World.Entities.Any(e => e is Player);
    public int DebugPlayerAttackCalls =>
    Player.DebugAttackCalls;
    public int DebugWeaponEvents =>
    Weapons.DebugWeaponEvents;
    public int DebugEventsAfterWeapons { get; private set; }
    private const float RestartProtectionDuration = 2f;
    public Game()
    {
        Player = new Player();

        World = new GameWorld();

        Movement = new MovementSystem();

        Combat = new CombatSystem();
        Weapons = new WeaponSystem();
        Cleanup = new CleanupSystem();
        Projectiles = new ProjectileSystem();
        Collision = new CollisionSystem();
        EnemyAI = new EnemyAISystem();
        Experience = new ExperienceSystem();
        Level = new LevelSystem();
        EnemySpawn = new EnemySpawnSystem();

        Wave = new WaveSystem(
            EnemySpawn, Combat);
        Map = new MapSystem();


        World.AddEntity(Player);
    }

    public void Start()
    {
        IsRunning = true;

        Player.Stats.RestoreFullHealth();
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Update(
    float delta,
    List<Vector2> spawnPositions)
    {
        if (RestartProtectionTimer > 0)
        {
            RestartProtectionTimer -= delta;
        }

        if (!IsRunning)
        {
            return;
        }

        if (RestartProtectionTimer > 0)
        {
            RestartProtectionTimer -= delta;

            if (RestartProtectionTimer < 0)
                RestartProtectionTimer = 0;
        }
        // =========================
        // GAME OVER BLOCK
        // =========================

        if (IsGameOver)
        {
            string state =
                $"GAME UPDATE BLOCKED\nSTATE Running={IsRunning} GameOver={IsGameOver}\n";


            if (state != _lastGameStateDebug)
            {
                Combat.DebugTag += state;

                _lastGameStateDebug = state;
            }

            return;
        }

        Combat.DebugTag = "";


        Combat.DebugTag +=
            "========== GAME UPDATE START ==========\n";


        Combat.DebugTag +=
            $"BEFORE CLEAR LastEvents={LastEvents.Count}\n";


        LastEvents.Clear();


        Combat.DebugTag +=
            $"AFTER CLEAR LastEvents={LastEvents.Count}\n";



        // =========================
        // SPAWN / WAVES
        // =========================


        Combat.DebugTag +=
            "BEFORE Wave.Update\n";


        Wave.Update(
            World,
            delta,
            spawnPositions);


        Combat.DebugTag +=
            "AFTER Wave.Update\n";



        if (Wave.IsWaveComplete)
        {
            Combat.DebugTag +=
                "BEFORE Start Next Wave\n";


            Wave.TryStartNextWave(
                World,
                Map,
                spawnPositions);


            Combat.DebugTag +=
                "AFTER Start Next Wave\n";
        }



        // =========================
        // AI
        // =========================


        Combat.DebugTag +=
            "BEFORE EnemyAI.Update\n";


        var aiEvents =
            EnemyAI.Update(
                World,
                Player,
                delta,
                RestartProtectionTimer);


        Combat.DebugTag +=
            $"AFTER EnemyAI.Update Events={aiEvents.Count}\n";


        LastEvents.AddRange(aiEvents);


        Combat.DebugTag +=
            $"After AI LastEvents={LastEvents.Count}\n";





        // =========================
        // MOVEMENT
        // =========================


        Combat.DebugTag +=
            "BEFORE Movement.Update\n";


        Movement.Update(
            World,
            delta);


        Combat.DebugTag +=
            "AFTER Movement.Update\n";





        // =========================
        // PLAYER WEAPONS
        // =========================


        Combat.DebugTag +=
            "BEFORE Player Weapons.Update\n";


        var playerWeaponEvents =
            Weapons.Update(
                Player,
                World,
                Combat,
                delta);



        Combat.DebugTag +=
            $"AFTER Player Weapons Events={playerWeaponEvents.Count}\n";


        LastEvents.AddRange(
            playerWeaponEvents);



        DebugEventsAfterWeapons =
            LastEvents.Count;


        Combat.DebugTag +=
            $"After Player Weapons LastEvents={LastEvents.Count}\n";





        // =========================
        // ENEMY WEAPONS
        // =========================


        Combat.DebugTag +=
            "BEFORE Enemy Weapons Loop\n";


        foreach (var entity in World.Entities)
        {
            if (entity is not Enemy enemy)
            {
                continue;
            }


            if (!enemy.Active || !enemy.IsAlive)
            {
                Combat.DebugTag +=
                    $"SKIP Enemy {enemy.Name} inactive\n";

                continue;
            }



            Combat.DebugTag +=
                $"BEFORE Enemy Weapon Update {enemy.Name}\n";



            if (RestartProtectionTimer <= 0)
            {
                var enemyWeaponEvents =
                    Weapons.Update(
                        enemy,
                        World,
                        Combat,
                        delta);

                LastEvents.AddRange(enemyWeaponEvents);
                Combat.DebugTag +=
               $"After Enemy Weapon LastEvents={LastEvents.Count}\n";
            }
            else
            {
                Combat.DebugTag +=
                    $"ENEMY ATTACK BLOCKED Restart Protection\n";
            }


           
        }



        Combat.DebugTag +=
            "AFTER Enemy Weapons Loop\n";





        // =========================
        // COLLISION
        // =========================


        Combat.DebugTag +=
            "BEFORE Collision.Update\n";


        var collisionEvents =
            Collision.Update(
                World,
                Combat);



        Combat.DebugTag +=
            $"AFTER Collision Events={collisionEvents.Count}\n";


        LastEvents.AddRange(
            collisionEvents);


        Combat.DebugTag +=
            $"After Collision LastEvents={LastEvents.Count}\n";





        // =========================
        // EXPERIENCE
        // =========================


        Combat.DebugTag +=
            "BEFORE Experience.Process\n";


        var experienceEvents =
            Experience.Process(
                Player,
                LastEvents);



        Combat.DebugTag +=
            $"AFTER Experience Events={experienceEvents.Count}\n";


        LastEvents.AddRange(
            experienceEvents);


        Combat.DebugTag +=
            $"After Experience LastEvents={LastEvents.Count}\n";





        // =========================
        // LEVEL
        // =========================


        Combat.DebugTag +=
            "BEFORE Level.Process\n";


        Level.Process(
            Player,
            LastEvents);


        Combat.DebugTag +=
            "AFTER Level.Process\n";





        // =========================
        // GAME OVER CHECK
        // =========================


        if (!Player.IsAlive)
        {
            IsGameOver = true;

            Player.Active = false;


            Combat.DebugTag +=
                "========== GAME OVER ==========\n";
        }





        // =========================
        // CLEANUP
        // =========================


        Combat.DebugTag +=
            "BEFORE Cleanup.Update\n";


        Cleanup.Update(
            World);


        Combat.DebugTag +=
            $"AFTER Cleanup Entities={World.Entities.Count}\n";



        Combat.DebugTag +=
            "========== GAME UPDATE END ==========\n";
    }
    
    public void Restart()
    {
        IsGameOver = false;
        IsRunning = true;

        RestartProtectionTimer = RestartProtectionDuration;

        RestartDebugTag =
            "========== RESTART ==========\n";

        Player.Revive();

        foreach (var weapon in Player.Weapons)
        {
            weapon.ResetCooldown();
        }
        RestartDebugTag +=
            $"REVIVE HP={Player.Stats.Health}/{Player.Stats.MaxHealth}\n";

        LastEvents.Clear();

        World.Clear();

        World.AddEntity(Player);

        Wave.Reset();

        Wave.StartWave(
            Map.GetCurrentWave()
        );
    }
}