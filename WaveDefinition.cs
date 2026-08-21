using System.Collections.Generic;

namespace RogueTest.Core.Entities;

public class WaveDefinition
{
    public List<EnemySpawnDefinition> Enemies { get; } = new();
}