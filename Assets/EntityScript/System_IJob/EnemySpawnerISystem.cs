using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemySpawnerISystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerFollowerSingletonIComponentData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        SpawnEnemy(ref state);
    }

    [BurstCompile]
    public void SpawnEnemy(ref SystemState state)
    {

        foreach (var (f, e) in SystemAPI.Query<RefRW<EnemySpawnerIComponentData>>()
                                                .WithEntityAccess())
        {

            DynamicBuffer<EnemyDataBufferElement> data = SystemAPI.GetSingletonBuffer<EnemyDataBufferElement>();
            //tao hang loat o day
            NativeArray<Entity> outputArray = new((int)(f.ValueRO.SpawnCount * data[f.ValueRO.IDEnemy].SpawnRate), Allocator.TempJob);
            state.EntityManager.Instantiate(data[f.ValueRO.IDEnemy].EntityPrefab, outputArray);

            EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                                               .CreateCommandBuffer(state.WorldUnmanaged);

            EnemySpawnerJob_FirstSetUp job = new EnemySpawnerJob_FirstSetUp
            {
                ECB = ecb.AsParallelWriter(),
                TargetArrayToSetUp = outputArray,
                SpawnSetting = SystemAPI.GetSingleton<EnemySpawnerSettingIComponentData>(),
                PlayerPos = GetPlayerPos(ref state),
                IDEnemy = f.ValueRO.IDEnemy,
            };

            JobHandle jobHandle = job.ScheduleParallel(outputArray.Length, 32, state.Dependency);
            state.Dependency = jobHandle;
            state.EntityManager.SetComponentEnabled<EnemySpawnerIComponentData>(e, false);
            outputArray.Dispose(jobHandle);
        }
    }

    public float3 GetPlayerPos(ref SystemState state)
    {
        foreach (var localTrs in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerFollowerSingletonIComponentData>())
        {
            return localTrs.ValueRO.Position;
        }
        return float3.zero;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

[BurstCompile]
public partial struct EnemySpawnerJob_FirstSetUp : IJobFor
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public NativeArray<Entity> TargetArrayToSetUp;
    [ReadOnly] public EnemySpawnerSettingIComponentData SpawnSetting;
    [ReadOnly] public float3 PlayerPos;
    [ReadOnly] public int IDEnemy;
    public void Execute(int index)
    {
        Random random = new Random((uint)SpawnSetting.RandomSeed * ((uint)index + 1));

        float rAngle = random.NextFloat(0, math.PI2);
        float rR = random.NextFloat(SpawnSetting.RMinSpawn, SpawnSetting.RMaxSpawn);

        float3 setPos = new float3
        (
            PlayerPos.x + rR * math.cos(rAngle),
            PlayerPos.y + rR * math.sin(rAngle),
            0f
        );

        ECB.SetComponent(index, TargetArrayToSetUp[index], new LocalTransform
        {
            Position = setPos,
            Rotation = quaternion.identity,
            Scale = 1f
        });
        ECB.SetComponent(index, TargetArrayToSetUp[index], new EnemyTag { ID = IDEnemy });
    }
}
