namespace RogueTest.Core.Entities;

public class MapDefinition
{
    public string Name { get; set; } = "Map";

    public List<WaveDefinition> Waves { get; } = new();



    public string DebugInfo()
    {
        string debug = "";

        debug +=
            "========== MAP DEFINITION ==========\n";


        debug +=
            $"Name={Name}\n";


        debug +=
            $"Waves Count={Waves.Count}\n";


        for (int i = 0; i < Waves.Count; i++)
        {
            debug +=
                $"Wave[{i}] Enemies={Waves[i].Enemies.Count}\n";
        }


        debug +=
            "========== END MAP DEFINITION ==========\n";


        return debug;
    }
}