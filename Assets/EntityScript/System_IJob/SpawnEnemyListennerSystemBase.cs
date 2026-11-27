using Unity.Entities;

public partial class SpawnEnemyListennerSystemBase : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        InvokeEventSpawnEnemy.InvokeEvent_SpawnEnemy += EventListenner;
    }
    protected override void OnUpdate()
    {

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        InvokeEventSpawnEnemy.InvokeEvent_SpawnEnemy -= EventListenner;
    }
    private void EventListenner(int idEnemy, int spawnCount, float rMinSpawn,float rMaxSpawn, int randomSeed)
    {
        foreach(var(data,entity) in SystemAPI.Query<RefRW<EnemySpawnerIComponentData>>()
                                             .WithEntityAccess()
                                             .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            data.ValueRW.SpawnCount = spawnCount;
            data.ValueRW.IDEnemy = idEnemy;
            EntityManager.SetComponentEnabled<EnemySpawnerIComponentData>(entity, true);
        }

        foreach (var (data, entity) in SystemAPI.Query<RefRW<EnemySpawnerSettingIComponentData>>().WithEntityAccess())
        {
            data.ValueRW.RMaxSpawn = rMaxSpawn;
            data.ValueRW.RMinSpawn = rMinSpawn;
            data.ValueRW.RandomSeed = randomSeed;
        }
    }
}
