using RogueTest.Core.Entities;
using RogueTest.Core.World;
using System.Numerics;

namespace RogueTest.Core.Systems;

public class MapSystem
{
    public MapDefinition? CurrentMap { get; private set; }

    public bool IsMapComplete { get; private set; }

    public int CurrentWaveIndex { get; private set; }


    // DEBUG
    public string DebugInfo { get; private set; } = "";


    public void StartMap(MapDefinition map)
    {
        DebugInfo = "";

        DebugInfo +=
            "========== MAP START ==========\n";


        CurrentMap = map;


        CurrentWaveIndex = 0;


        IsMapComplete = false;


        DebugInfo +=
            $"Map Loaded: {map}\n";


        DebugInfo +=
            $"Waves Count={map.Waves.Count}\n";


        DebugInfo +=
            $"Current Wave Index={CurrentWaveIndex}\n";


        DebugInfo +=
            "========== MAP START END ==========\n";
    }



    public WaveDefinition? GetCurrentWave()
    {
        DebugInfo = "";


        DebugInfo +=
            "========== GET CURRENT WAVE ==========\n";


        if (CurrentMap == null)
        {
            DebugInfo +=
                "FAIL: CurrentMap null\n";

            return null;
        }


        DebugInfo +=
            $"Wave Index={CurrentWaveIndex}\n";


        DebugInfo +=
            $"Total Waves={CurrentMap.Waves.Count}\n";



        if (CurrentWaveIndex < 0 ||
            CurrentWaveIndex >= CurrentMap.Waves.Count)
        {
            DebugInfo +=
                "FAIL: Index out of range\n";

            return null;
        }



        WaveDefinition wave =
            CurrentMap.Waves[CurrentWaveIndex];


        DebugInfo +=
            "Wave Found\n";


        DebugInfo +=
            $"Enemies Definitions={wave.Enemies.Count}\n";


        DebugInfo +=
            "========== GET CURRENT WAVE END ==========\n";


        return wave;
    }





    public bool HasNextWave()
    {
        DebugInfo = "";


        DebugInfo +=
            "========== HAS NEXT WAVE ==========\n";


        if (CurrentMap == null)
        {
            DebugInfo +=
                "FAIL: CurrentMap null\n";

            return false;
        }



        bool result =
            CurrentWaveIndex + 1 <
            CurrentMap.Waves.Count;



        DebugInfo +=
            $"Current={CurrentWaveIndex}\n";


        DebugInfo +=
            $"Total={CurrentMap.Waves.Count}\n";


        DebugInfo +=
            $"Result={result}\n";


        return result;
    }





    public List<Enemy> SpawnCurrentWave(
        GameWorld world,
        WaveSystem waveSystem,
        List<Vector2> positions)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== SPAWN CURRENT WAVE ==========\n";


        WaveDefinition? wave =
            GetCurrentWave();



        if (wave == null)
        {
            DebugInfo +=
                "FAIL: No current wave\n";

            return new List<Enemy>();
        }



        DebugInfo +=
            "Calling WaveSystem.SpawnWave\n";



        List<Enemy> enemies =
            waveSystem.SpawnWave(
                world,
                wave,
                positions);



        DebugInfo +=
            $"Spawned Enemies={enemies.Count}\n";


        DebugInfo +=
            $"World Entities={world.Entities.Count}\n";


        DebugInfo +=
            "========== SPAWN CURRENT WAVE END ==========\n";


        return enemies;
    }





    public bool TryAdvanceWave()
    {
        DebugInfo = "";


        DebugInfo +=
            "========== ADVANCE WAVE ==========\n";


        if (!HasNextWave())
        {
            IsMapComplete = true;


            DebugInfo +=
                "MAP COMPLETE\n";


            return false;
        }



        CurrentWaveIndex++;


        DebugInfo +=
            $"New Wave Index={CurrentWaveIndex}\n";


        return true;
    }
}