using RogueTest.Core.Entities;
using RogueTest.Core.World;

namespace RogueTest.Core.Systems;

public class CleanupSystem
{
    public string DebugInfo { get; private set; } = "";



    public void Update(GameWorld world)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== CLEANUP START ==========\n";


        DebugInfo +=
            $"BEFORE Entity Count={world.Entities.Count}\n";



        DebugInfo +=
            "BEFORE Create Remove List\n";


        List<Entities.Entity> entitiesToRemove =
            new();


        DebugInfo +=
            "AFTER Create Remove List\n";



        DebugInfo +=
            "BEFORE Scan Entities\n";


        foreach (var entity in world.Entities)
        {
            DebugInfo +=
                $"CHECK Entity={entity.GetType().Name} Active={entity.Active}\n";


            if (!entity.Active)
            {
                // =========================
                // KEEP PLAYER AFTER DEATH
                // =========================

                if (entity is Player)
                {
                    DebugInfo +=
                        "KEEP PLAYER - GAME OVER\n";

                    continue;
                }


                DebugInfo +=
                    $"MARK REMOVE {entity.GetType().Name}\n";


                entitiesToRemove.Add(entity);
            }
        }


        DebugInfo +=
            $"AFTER Scan Entities Marked={entitiesToRemove.Count}\n";



        DebugInfo +=
            "BEFORE Remove Entities\n";


        foreach (var entity in entitiesToRemove)
        {
            DebugInfo +=
                $"BEFORE Remove {entity.GetType().Name}\n";


            world.RemoveEntity(entity);


            DebugInfo +=
                $"AFTER Remove {entity.GetType().Name}\n";
        }



        DebugInfo +=
            $"AFTER Remove Entity Count={world.Entities.Count}\n";


        DebugInfo +=
            "========== CLEANUP END ==========\n";
    }
}