using RogueTest.Core.Entities;
using RogueTest.Core.World;
using System.Numerics;

namespace RogueTest.Core.Systems;

public class MapSystem
{
    public MapDefinition? CurrentMap { get; private set; }
    public bool IsMapComplete { get; private set; }
    public int CurrentWaveIndex { get; private set; }

    public void StartMap(MapDefinition map)
    {
        CurrentMap = map;
        CurrentWaveIndex = 0;
        IsMapComplete = false;
    }

    public WaveDefinition? GetCurrentWave()
    {
        if (CurrentMap == null)
            return null;

        if (CurrentWaveIndex < 0 ||
            CurrentWaveIndex >= CurrentMap.Waves.Count)
            return null;

        return CurrentMap.Waves[CurrentWaveIndex];
    }
    public bool HasNextWave()
    {
        if (CurrentMap == null)
            return false;

        return CurrentWaveIndex + 1 <
               CurrentMap.Waves.Count;
    }
    public List<Enemy> SpawnCurrentWave(
    GameWorld world,
    WaveSystem waveSystem,
    List<Vector2> positions)
    {
        WaveDefinition? wave =
            GetCurrentWave();

        if (wave == null)
            return new List<Enemy>();

        return waveSystem.SpawnWave(
            world,
            wave,
            positions);
    }
    public bool TryAdvanceWave()
    {
        if (!HasNextWave())
        {
            IsMapComplete = true;
            return false;
        }

        CurrentWaveIndex++;

        return true;
    }

}