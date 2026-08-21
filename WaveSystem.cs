using System.Numerics;
using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class WaveSystem
{
    private readonly EnemySpawnSystem _enemySpawnSystem;
    private readonly CombatSystem _combat;

    private readonly List<Enemy> _currentWaveEnemies = new();

    public IReadOnlyList<Enemy> CurrentWaveEnemies =>
        _currentWaveEnemies;


    public bool IsWaveComplete =>
        !_spawningWave &&
        _currentWaveEnemies.Count > 0 &&
        _currentWaveEnemies.All(enemy => !enemy.IsAlive);


    private WaveDefinition? _currentWaveDefinition;

    private int _nextEnemyIndex;

    private float _spawnTimer;

    private bool _spawningWave;

    public bool IsSpawningWave =>
        _spawningWave;


    public int NextEnemyIndex =>
        _nextEnemyIndex;


    public float SpawnTimer =>
        _spawnTimer;


    private int _currentSpawnDefinitionIndex;

    private int _currentSpawnCount;


    public int CurrentSpawnDefinitionIndex =>
        _currentSpawnDefinitionIndex;


    public int CurrentSpawnCount =>
        _currentSpawnCount;

    public WaveSystem(
        EnemySpawnSystem enemySpawnSystem,
    CombatSystem combat)
    {
        _enemySpawnSystem = enemySpawnSystem;

        _combat = combat;
    }

    public List<Enemy> SpawnWave(
    GameWorld world,
    WaveDefinition definition,
    List<Vector2> positions)
    {
        _currentWaveEnemies.Clear();

        List<Enemy> spawnedEnemies = new();

        int positionIndex = 0;


        foreach (EnemySpawnDefinition spawnDefinition
                 in definition.Enemies)
        {
            for (int i = 0;
                 i < spawnDefinition.Count;
                 i++)
            {
                if (positions.Count == 0)
                    break;


                Vector2 position =
                    positions[positionIndex % positions.Count];


                Enemy enemy =
                    _enemySpawnSystem.Spawn(
                        world,
                        spawnDefinition.Enemy,
                        position,
                        _combat);


                spawnedEnemies.Add(enemy);

                _currentWaveEnemies.Add(enemy);

                positionIndex++;
            }
        }


        return spawnedEnemies;
    }
    public bool TryStartNextWave(
    GameWorld world,
    MapSystem mapSystem,
    List<Vector2> positions)
    {
        if (!IsWaveComplete)
            return false;

        if (!mapSystem.TryAdvanceWave())
            return false;

        WaveDefinition? nextWave =
            mapSystem.GetCurrentWave();

        if (nextWave == null)
            return false;

        SpawnWave(
            world,
            nextWave,
            positions);

        return true;
    }
    public void StartWave(WaveDefinition definition)
    {
        _currentWaveDefinition = definition;

        _currentSpawnDefinitionIndex = 0;
        _currentSpawnCount = 0;

        _spawnTimer = 0;

        _spawningWave = true;

        _currentWaveEnemies.Clear();
    }
    public void Update(
    GameWorld world,
    float delta,
    List<Vector2> positions)
    {
        if (!_spawningWave)
            return;

        if (_currentWaveDefinition == null)
            return;

        _spawnTimer += delta;

        EnemySpawnDefinition definition =
            _currentWaveDefinition.Enemies[
                _currentSpawnDefinitionIndex];

        if (_spawnTimer < definition.SpawnInterval)
            return;

        _spawnTimer = 0;

        SpawnNextEnemy(
            world,
            positions);
    }
    private bool SpawnNextEnemy(
    GameWorld world,
    List<Vector2> positions)
    {
        if (_currentWaveDefinition == null)
            return false;

        while (_currentSpawnDefinitionIndex <
               _currentWaveDefinition.Enemies.Count)
        {
            EnemySpawnDefinition definition =
                _currentWaveDefinition.Enemies[
                    _currentSpawnDefinitionIndex];

            // Terminamos este tipo de enemigo.
            if (_currentSpawnCount >= definition.Count)
            {
                _currentSpawnDefinitionIndex++;
                _currentSpawnCount = 0;
                continue;
            }

            if (positions.Count == 0)
                return false;

            Vector2 position =
                positions[
                    _currentWaveEnemies.Count %
                    positions.Count];

            Enemy enemy =
                _enemySpawnSystem.Spawn(
                    world,
                    definition.Enemy,
                    position,
                    _combat);

            _currentWaveEnemies.Add(enemy);

            _currentSpawnCount++;

            // ¿Terminamos absolutamente todos
            // los EnemySpawnDefinition?
            if (_currentSpawnCount >= definition.Count &&
                _currentSpawnDefinitionIndex + 1 >=
                _currentWaveDefinition.Enemies.Count)
            {
                _spawningWave = false;
            }

            return true;
        }

        _spawningWave = false;

        return false;
    }
}