using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public class Entities : SystemBase
{
    public EntityManager entity_manager;
    
    protected override void OnCreate()
    {
        entity_manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Entity plank = entity_manager.CreateEntity(); // Create a single entity


        EntityArchetype Plank_Archetype = entity_manager.CreateArchetype( // Create an archetype
		typeof(Plank),
		typeof(Health),
        typeof(Size)
        );

        EntityArchetype Floor_Board_Archetype = entity_manager.CreateArchetype( // Create an archetype
		typeof(FloorBoard),
		typeof(Health),
        typeof(Size)
        );

        Entity plank = entity_manager.CreateEntity(Plank_Archetype); // Create an entity based on archetype
        Entity floor_board = entity_manager.CreateEntity(Floor_Board_Archetype); // Create an entity based on archetype
        
        // Create an array of Entities
        NativeArray<Entity> plank_entities = new NativeArray<Entity>(1_000_000, Allocator.Temp);
        entity_manager.CreateEntity(Plank_Archetype, plank_entities);

        Entities.ForEach((ref Health health) =>
        {
            var old_health = health;
            old_health.value = 100;
            health = old_health;
            // Debug.Log(health);
        }).Schedule();

    }
    
    protected override void OnUpdate()
    {
        Entities.ForEach(( ref Health health ) => {
            int health_value = health.value;
            if (health_value > 0){
                int new_health = health_value - 1;
                health.value = new_health;
                if (new_health == 0) {
                    // Grab the specific entity and add a death component to it
                    Debug.Log("DIED");
                } 
            }
        }).Schedule();
    }

}

// TODO: Figure out how to use queries and then operate ForEach on a query