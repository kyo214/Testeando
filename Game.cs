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
        if (!IsRunning)
            return;


        // =========================
        // CLEAR EVENTS FROM PREVIOUS FRAME
        // =========================

        LastEvents.Clear();



        // =========================
        // SPAWN / WAVES
        // =========================

        Wave.Update(
            World,
            delta,
            spawnPositions);


        if (Wave.IsWaveComplete)
        {
            Wave.TryStartNextWave(
                World,
                Map,
                spawnPositions);
        }



        // =========================
        // AI
        // =========================

        LastEvents.AddRange(
    EnemyAI.Update(
        World,
        Player,
        delta));



        // =========================
        // MOVEMENT
        // =========================

        Movement.Update(
            World,
            delta);



        // =========================
        // PLAYER WEAPONS
        // =========================

        LastEvents.AddRange(
            Weapons.Update(
                Player,
                World,
                Combat,
                delta));


        DebugEventsAfterWeapons = LastEvents.Count;
        // =========================
        // ENEMY WEAPONS
        // =========================

        foreach (var entity in World.Entities)
        {
            if (entity is not Enemy enemy)
                continue;


            if (!enemy.Active || !enemy.IsAlive)
                continue;


            LastEvents.AddRange(
                Weapons.Update(
                    enemy,
                    World,
                    Combat,
                    delta));
        }



        // =========================
        // COLLISION
        // =========================

        LastEvents.AddRange(
            Collision.Update(
                World,
                Combat));



        // =========================
        // EXPERIENCE
        // =========================

        LastEvents.AddRange(
            Experience.Process(
                Player,
                LastEvents));



        // =========================
        // LEVEL
        // =========================

        Level.Process(
            Player,
            LastEvents);



        // =========================
        // CLEANUP
        // =========================

        Cleanup.Update(
            World);
    }
}