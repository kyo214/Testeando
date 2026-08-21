namespace RogueTest.Core.Entities;

public class EnemySpawnDefinition
{
    public EnemyDefinition Enemy { get; set; } = null!;
    public float SpawnInterval { get; set; }
    public int Count { get; set; } = 1;
}