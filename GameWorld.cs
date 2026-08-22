using RogueTest.Core.Entities;

namespace RogueTest.Core.World;

public class GameWorld
{
    private readonly List<Entity> _entities = new();


    public IReadOnlyList<Entity> Entities =>
        _entities;



    public string DebugInfo { get; private set; } = "";



    public void AddEntity(Entity entity)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== WORLD ADD ENTITY ==========\n";


        DebugInfo +=
            $"TRY ADD {entity.GetType().Name}\n";


        DebugInfo +=
            $"Before Count={_entities.Count}\n";



        if (!_entities.Contains(entity))
        {
            _entities.Add(entity);


            DebugInfo +=
                "ENTITY ADDED\n";
        }
        else
        {
            DebugInfo +=
                "ENTITY ALREADY EXISTS\n";
        }



        DebugInfo +=
            $"After Count={_entities.Count}\n";
    }





    public void RemoveEntity(Entity entity)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== WORLD REMOVE ENTITY ==========\n";


        DebugInfo +=
            $"REMOVE {entity.GetType().Name}\n";


        DebugInfo +=
            $"Before Count={_entities.Count}\n";



        bool removed =
            _entities.Remove(entity);



        DebugInfo +=
            $"Removed={removed}\n";


        DebugInfo +=
            $"After Count={_entities.Count}\n";
    }





    public void Clear()
    {
        DebugInfo = "";


        DebugInfo +=
            $"WORLD CLEAR Count Before={_entities.Count}\n";


        _entities.Clear();


        DebugInfo +=
            $"WORLD CLEAR Count After={_entities.Count}\n";
    }





    public void Update(float delta)
    {
        DebugInfo = "";


        DebugInfo +=
            "========== WORLD UPDATE ==========\n";


        DebugInfo +=
            $"Entities={_entities.Count}\n";



        foreach (Entity entity in _entities)
        {
            DebugInfo +=
                $"CHECK {entity.GetType().Name} Active={entity.Active}\n";


            if (entity.Active)
            {
                entity.Update(delta);
            }
            else
            {
                DebugInfo +=
                    "SKIP INACTIVE\n";
            }
        }


        DebugInfo +=
            "========== END WORLD UPDATE ==========\n";
    }
}