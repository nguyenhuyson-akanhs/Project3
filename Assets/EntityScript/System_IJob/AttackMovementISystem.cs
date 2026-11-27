using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct AttackMovementISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AttackTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb1 = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                                   .CreateCommandBuffer(state.WorldUnmanaged);

        AttackMovementJob job = new AttackMovementJob
        {
            BeginECB = ecb1.AsParallelWriter(),
            DeltaTime = SystemAPI.Time.DeltaTime,
            Data = SystemAPI.GetSingletonBuffer<AttackDataIBufferElement>(true)
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

[BurstCompile]
[WithNone(typeof(DestroyTag))]
public partial struct AttackMovementJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter BeginECB;
    [ReadOnly] public DynamicBuffer<AttackDataIBufferElement> Data;

    [BurstCompile]
    public void Execute([ChunkIndexInQuery] int index, Entity entity, RefRO<LocalTransform> localTrs,
                                                                      RefRO<AttackMovementIComponentData> movementData,
                                                                      RefRO<AttackTag> attackTag)
    {
        if(movementData.ValueRO.TimeExist <= 0) // okenow
        {
            BeginECB.SetComponentEnabled<DestroyTag>(index, entity, true);
            return;
        }

        BeginECB.SetComponent(index, entity, LocalTransform.FromPositionRotationScale
        (
            localTrs.ValueRO.Position + movementData.ValueRO.Direction * Data[attackTag.ValueRO.ID].NormalSpeed * DeltaTime,
            math.mul(localTrs.ValueRO.Rotation, quaternion.RotateZ(Data[attackTag.ValueRO.ID].NormalRotateSpeed * DeltaTime)),
            Data[attackTag.ValueRO.ID].NormalScale
        ));
        BeginECB.SetComponent(index, entity, new AttackMovementIComponentData
        {
            Direction = movementData.ValueRO.Direction,
            TimeExist = movementData.ValueRO.TimeExist - DeltaTime,
        });
    }
}
// try use tag to destroy them