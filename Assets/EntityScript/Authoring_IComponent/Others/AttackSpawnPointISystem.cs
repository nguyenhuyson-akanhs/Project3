using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct AttackSpawnPointISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AttackSpawnPointPrefabsSingletonIComponentData>();
        state.RequireForUpdate<AttackSpawnPointStatsSingletonIComponentData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int count = 0;
        int currentIndex = 0;
        float3 playerPos = float3.zero;
        AttackSpawnPointStatsSingletonIComponentData aspStats = SystemAPI.GetSingleton<AttackSpawnPointStatsSingletonIComponentData>();
        foreach (RefRO<AttackSpawnPointTag> t in SystemAPI.Query<RefRO<AttackSpawnPointTag>>()) count++;
        foreach (RefRW<LocalTransform> localTrs in SystemAPI.Query<RefRW<LocalTransform>>()
                                                            .WithAll<PlayerFollowerSingletonIComponentData>()) playerPos = localTrs.ValueRO.Position;
        foreach (RefRW<LocalTransform> localTrs in SystemAPI.Query<RefRW<LocalTransform>>()
                                                            .WithAll<AttackSpawnPointTag>())
        {
            currentIndex++;
            localTrs.ValueRW.Position = new float3
            (
                playerPos.x 
                + aspStats.Radius * math.cos(math.radians(aspStats.RotatingSpeed * (float)SystemAPI.Time.ElapsedTime 
                + (float)currentIndex * 360f / count)),

                playerPos.y 
                + aspStats.Radius * math.sin(math.radians(aspStats.RotatingSpeed * (float)SystemAPI.Time.ElapsedTime 
                + (float)currentIndex * 360f / count)),

                0f
            );
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
