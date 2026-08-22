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



    public string DebugInfo { get; private set; } = "";



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

        DebugInfo = "";

        DebugInfo +=
            "========== SPAWN WAVE START ==========\n";


        DebugInfo +=
            $"Enemies Definitions={definition.Enemies.Count}\n";


        DebugInfo +=
            $"Positions Count={positions.Count}\n";



        _currentWaveEnemies.Clear();


        List<Enemy> spawnedEnemies =
            new();



        int positionIndex = 0;



        foreach (EnemySpawnDefinition spawnDefinition
                 in definition.Enemies)
        {

            DebugInfo +=
                $"Spawn Definition Enemy={spawnDefinition.Enemy.Name} Count={spawnDefinition.Count}\n";



            for (int i = 0;
                 i < spawnDefinition.Count;
                 i++)
            {

                if (positions.Count == 0)
                {
                    DebugInfo +=
                        "STOP No Spawn Positions\n";

                    break;
                }



                Vector2 position =
                    positions[positionIndex % positions.Count];



                DebugInfo +=
                    $"CALL EnemySpawn Position={position}\n";



                Enemy enemy =
                    _enemySpawnSystem.Spawn(
                        world,
                        spawnDefinition.Enemy,
                        position,
                        _combat);



                DebugInfo +=
                    $"SPAWNED Enemy={enemy.Name}\n";



                spawnedEnemies.Add(enemy);


                _currentWaveEnemies.Add(enemy);



                DebugInfo +=
                    $"Current Wave Enemies={_currentWaveEnemies.Count}\n";



                positionIndex++;
            }
        }



        DebugInfo +=
            $"RETURN Spawned={spawnedEnemies.Count}\n";


        DebugInfo +=
            "========== SPAWN WAVE END ==========\n";


        return spawnedEnemies;
    }







    public bool TryStartNextWave(
    GameWorld world,
    MapSystem mapSystem,
    List<Vector2> positions)
    {
        DebugInfo +=
            "========== TRY NEXT WAVE ==========\n";


        DebugInfo +=
            $"IsWaveComplete={IsWaveComplete}\n";


        DebugInfo +=
            $"SpawningWave={_spawningWave}\n";


        DebugInfo +=
            $"CurrentWaveEnemies={_currentWaveEnemies.Count}\n";


        DebugInfo +=
            $"MapWaveIndex={mapSystem.CurrentWaveIndex}\n";



        if (!IsWaveComplete)
        {
            DebugInfo +=
                "BLOCKED Wave not complete\n";

            return false;
        }



        if (!mapSystem.TryAdvanceWave())
        {
            DebugInfo +=
                "BLOCKED Cannot advance map\n";

            return false;
        }



        DebugInfo +=
            $"ADVANCED MapWaveIndex={mapSystem.CurrentWaveIndex}\n";



        WaveDefinition? nextWave =
            mapSystem.GetCurrentWave();



        if (nextWave == null)
        {
            DebugInfo +=
                "BLOCKED Next wave null\n";

            return false;
        }



        DebugInfo +=
            $"NEXT WAVE Enemy Definitions={nextWave.Enemies.Count}\n";



        List<Enemy> enemies =
            SpawnWave(
                world,
                nextWave,
                positions);



        DebugInfo +=
            $"SpawnWave Returned={enemies.Count}\n";


        DebugInfo +=
            "NEXT WAVE STARTED\n";


        return true;
    }







    public void StartWave(
    WaveDefinition definition)
    {

        DebugInfo +=
            "========== START WAVE ==========\n";


        DebugInfo +=
            $"Before Spawning={_spawningWave}\n";


        DebugInfo +=
            $"Before Enemies={_currentWaveEnemies.Count}\n";


        DebugInfo +=
            $"Definitions={definition.Enemies.Count}\n";



        _currentWaveDefinition =
            definition;



        _currentSpawnDefinitionIndex = 0;


        _currentSpawnCount = 0;


        _spawnTimer = 0;


        _spawningWave = true;


        _currentWaveEnemies.Clear();



        DebugInfo +=
            $"After Spawning={_spawningWave}\n";


        DebugInfo +=
            $"After Enemies={_currentWaveEnemies.Count}\n";


        DebugInfo +=
            "Wave Ready For Spawning\n";
    }







    public void Update(
        GameWorld world,
        float delta,
        List<Vector2> positions)
    {

        DebugInfo = "";

        DebugInfo +=
            "========== WAVE UPDATE ==========\n";



        DebugInfo +=
            $"Spawning={_spawningWave}\n";


        DebugInfo +=
            $"Timer={_spawnTimer}\n";



        if (!_spawningWave)
        {
            DebugInfo +=
                "RETURN Not Spawning\n";

            return;
        }



        if (_currentWaveDefinition == null)
        {
            DebugInfo +=
                "RETURN Definition NULL\n";

            return;
        }



        _spawnTimer += delta;



        DebugInfo +=
            $"Timer After Add={_spawnTimer}\n";



        EnemySpawnDefinition definition =
            _currentWaveDefinition.Enemies[
                _currentSpawnDefinitionIndex];



        DebugInfo +=
            $"Current Enemy={definition.Enemy.Name}\n";



        if (_spawnTimer < definition.SpawnInterval)
        {
            DebugInfo +=
                "WAIT Spawn Interval\n";

            return;
        }



        _spawnTimer = 0;



        DebugInfo +=
            "CALL SpawnNextEnemy\n";



        SpawnNextEnemy(
            world,
            positions);
    }







    private bool SpawnNextEnemy(
        GameWorld world,
        List<Vector2> positions)
    {

        DebugInfo +=
        "========== SPAWN NEXT ENEMY ==========\n";


        DebugInfo +=
            $"Index={_currentSpawnDefinitionIndex}\n";


        DebugInfo +=
            $"SpawnCount={_currentSpawnCount}\n";


        DebugInfo +=
            $"Definitions={_currentWaveDefinition?.Enemies.Count}\n";




        if (_currentWaveDefinition == null)
        {
            DebugInfo +=
                "BLOCKED Definition NULL\n";

            return false;
        }



        while (_currentSpawnDefinitionIndex <
               _currentWaveDefinition.Enemies.Count)
        {

            EnemySpawnDefinition definition =
                _currentWaveDefinition.Enemies[
                    _currentSpawnDefinitionIndex];



            DebugInfo +=
                $"Definition Index={_currentSpawnDefinitionIndex}\n";



            DebugInfo +=
                $"Spawn Count={_currentSpawnCount}/{definition.Count}\n";



            if (_currentSpawnCount >= definition.Count)
            {
                DebugInfo +=
                    "NEXT Definition\n";


                _currentSpawnDefinitionIndex++;

                _currentSpawnCount = 0;

                continue;
            }



            if (positions.Count == 0)
            {
                DebugInfo +=
                    "BLOCKED No Positions\n";

                return false;
            }



            Vector2 position =
                positions[
                    _currentWaveEnemies.Count %
                    positions.Count];



            DebugInfo +=
                $"CALL EnemySpawn Position={position}\n";



            Enemy enemy =
                _enemySpawnSystem.Spawn(
                    world,
                    definition.Enemy,
                    position,
                    _combat);



            DebugInfo +=
                $"SPAWN OK Enemy={enemy.Name}\n";



            _currentWaveEnemies.Add(enemy);



            _currentSpawnCount++;



            DebugInfo +=
                $"CurrentWaveEnemies={_currentWaveEnemies.Count}\n";



            if (_currentSpawnCount >= definition.Count &&
                _currentSpawnDefinitionIndex + 1 >=
                _currentWaveDefinition.Enemies.Count)
            {
                _spawningWave = false;


                DebugInfo +=
                    "SPAWNING COMPLETE\n";
            }



            return true;
        }



        _spawningWave = false;



        DebugInfo +=
            "NO MORE ENEMIES\n";


        return false;
    }
    public void Reset()
    {
        DebugInfo +=
            "========== RESET WAVE ==========\n";


        _currentWaveEnemies.Clear();

        _currentWaveDefinition = null;

        _nextEnemyIndex = 0;

        _spawnTimer = 0;

        _spawningWave = false;

        _currentSpawnDefinitionIndex = 0;

        _currentSpawnCount = 0;


        DebugInfo +=
            "Wave Reset Complete\n";
    }
}