using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    
}

public class EnemySpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new EnemySpawnerIComponentData { });
        SetComponentEnabled<EnemySpawnerIComponentData>(entity, false);
        AddComponent(entity, new EnemySpawnerSettingIComponentData { });
    }
}
public struct EnemySpawnerIComponentData : IComponentData, IEnableableComponent
{
    public int IDEnemy;
    public int SpawnCount;
}

public struct EnemySpawnerSettingIComponentData : IComponentData
{
    public float RMinSpawn;
    public float RMaxSpawn;
    public int RandomSeed;
}
