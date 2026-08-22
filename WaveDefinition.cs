using System.Collections.Generic;

namespace RogueTest.Core.Entities;

public class WaveDefinition
{
    public List<EnemySpawnDefinition> Enemies { get; } = new();



    public string DebugInfo()
    {
        string debug = "";

        debug +=
            "========== WAVE DEFINITION ==========\n";


        debug +=
            $"Enemy Definitions Count={Enemies.Count}\n";


        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemySpawnDefinition enemy =
                Enemies[i];


            debug +=
                $"[{i}] Enemy={enemy.Enemy.Name} Count={enemy.Count}\n";
        }


        debug +=
            "========== END WAVE DEFINITION ==========\n";


        return debug;
    }
}