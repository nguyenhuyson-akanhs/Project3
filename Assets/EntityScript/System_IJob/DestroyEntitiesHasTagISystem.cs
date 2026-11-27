using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(AttackMovementISystem))]
partial struct DestroyEntitiesHasTagISystem : ISystem
{
    public EntityQuery QueryEntityReadyToDestroy;
    public int ThresholdFrameToDestroy;
    public int CurrentFrameToDestroy;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DestroyTag>();

        QueryEntityReadyToDestroy = new EntityQueryBuilder(Allocator.Temp)
                                .WithAll<DestroyTag>()
                                .Build(ref state);

        ThresholdFrameToDestroy = 10;
        CurrentFrameToDestroy = 0;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        DestroyEntites(ref state);
    }

    [BurstCompile]
    public void DestroyEntites(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                                           .CreateCommandBuffer(state.WorldUnmanaged);

        CurrentFrameToDestroy++;
        if (CurrentFrameToDestroy == ThresholdFrameToDestroy)
        {
            CurrentFrameToDestroy = 0;
            if (!QueryEntityReadyToDestroy.IsEmpty)
            {
                ecb.DestroyEntity(QueryEntityReadyToDestroy,EntityQueryCaptureMode.AtPlayback);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
