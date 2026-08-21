namespace RogueTest.Core.Entities;

public class MapDefinition
{
    public string Name { get; set; } = "Map";

    public List<WaveDefinition> Waves { get; } = new();
}