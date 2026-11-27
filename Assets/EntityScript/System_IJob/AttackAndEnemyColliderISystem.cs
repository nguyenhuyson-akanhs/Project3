using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
//[UpdateAfter(typeof(SimulationSystemGroup))]
public partial struct AttackAndEnemyColliderISystem : ISystem
{
    public ComponentLookup<EnemyTag> LookupEnemy;
    public ComponentLookup<AttackTag> LookupAttack;
    //public ComponentLookup<DestroyTag> LookupDestroy;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
        state.RequireForUpdate<AttackTag>();

        LookupEnemy = state.GetComponentLookup<EnemyTag>();
        LookupAttack = state.GetComponentLookup<AttackTag>();
        //LookupDestroy = state.GetComponentLookup<DestroyTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //UnityEngine.Debug.Log("Looping!");

        LookupEnemy.Update(ref state);
        LookupAttack.Update(ref state);
        //LookupDestroy.Update(ref state);

        var job = new AttackAndEnemyColliderJob
        {
            ECB = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                           .CreateCommandBuffer(state.WorldUnmanaged)
                           .AsParallelWriter(),
            LookupEnemy = LookupEnemy,
            LookupAttack = LookupAttack,
            //LookupDestroy = LookupDestroy,
        };

        state.Dependency = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    public partial struct AttackAndEnemyColliderJob : ITriggerEventsJob
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        [ReadOnly] public ComponentLookup<EnemyTag> LookupEnemy;
        [ReadOnly] public ComponentLookup<AttackTag> LookupAttack;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity A = triggerEvent.EntityA;
            Entity B = triggerEvent.EntityB;

            if (LookupEnemy.TryGetComponent(A, out _) && LookupAttack.TryGetComponent(B, out _))
            {
                //UnityEngine.Debug.Log($"Enemy {A}, Attack {B}");
                ECB.SetComponentEnabled<DestroyTag>(triggerEvent.BodyIndexA, A, true);
            }
            else if (LookupAttack.TryGetComponent(A, out _) && LookupEnemy.TryGetComponent(B, out _))
            {
                //UnityEngine.Debug.Log($"Enemy {B}, Attack {A}");
                ECB.SetComponentEnabled<DestroyTag>(triggerEvent.BodyIndexB, B, true);
            }
        }
    }
}


