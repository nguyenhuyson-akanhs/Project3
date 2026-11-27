using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MoveWithInputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var(inputData,playerData,localTrs) in SystemAPI.Query<RefRO<InputsIComponentData>,
                                                                      RefRO<PlayerIComponentData>,
                                                                      RefRW<LocalTransform>>())
        {
            float3 position = float3.zero;
            position.x = localTrs.ValueRW.Position.x + inputData.ValueRO.MoveDirection.x * playerData.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;
            position.y = localTrs.ValueRW.Position.y + inputData.ValueRO.MoveDirection.y * playerData.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;
            localTrs.ValueRW.Position = position;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
