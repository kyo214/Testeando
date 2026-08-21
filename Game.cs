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
            EnemySpawn);
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

    public void Update(float delta)
    {
        if (!IsRunning)
            return;

        LastEvents.Clear();

        EnemyAI.Update(
            World,
            Player,
            delta);

        Movement.Update(
            World,
            delta);

        LastEvents.AddRange(
            Weapons.Update(
                Player,
                World,
                Combat,
                delta));

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

        Projectiles.Update(
            World,
            delta);

        LastEvents.AddRange(
            Collision.Update(
                World,
                Combat));

        Cleanup.Update(
            World);


        // =========================
        // EXPERIENCE
        // =========================

        LastEvents.AddRange(
            Experience.Process(
                Player,
                LastEvents));
        Level.Process(
    Player,
    LastEvents);
    }
    public void Update(
     float delta,
     List<System.Numerics.Vector2> spawnPositions)
    {
        if (Map.IsMapComplete)
            return;

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
    }
}