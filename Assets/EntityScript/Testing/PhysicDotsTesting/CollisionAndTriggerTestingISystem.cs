//using Unity.Burst;
//using Unity.Entities;
//using Unity.Physics;
//using Unity.Physics.Systems;

//[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
//[UpdateAfter(typeof(PhysicsSystemGroup))]
//[UpdateAfter(typeof(PhysicTestingISystem))]
//public partial struct CollisionAndTriggerTestingISystem : ISystem
//{
//    [BurstCompile]
//    public void OnCreate(ref SystemState state)
//    {

//    }

//    [BurstCompile]
//    public void OnUpdate(ref SystemState state)
//    {
//        var job = new CollisionJob
//        {
//            ECB = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
//                           .CreateCommandBuffer(state.WorldUnmanaged)
//                           .AsParallelWriter()
//        };
//        state.Dependency = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
//        //var job2 = new TestJob { };
//        //state.Dependency = job2.ScheduleParallel(state.Dependency);
//        var job2 = new TriggerJob
//        {
//            ECB = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
//                   .CreateCommandBuffer(state.WorldUnmanaged)
//                   .AsParallelWriter()
//        };
//        state.Dependency = job2.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
//    }

//    [BurstCompile]
//    public void OnDestroy(ref SystemState state)
//    {
        
//    }

//    public partial struct CollisionJob : ICollisionEventsJob
//    {
//        public EntityCommandBuffer.ParallelWriter ECB;
//        public void Execute(CollisionEvent collisionEvent)
//        {
//            Entity A = collisionEvent.EntityA;
//            Entity B = collisionEvent.EntityB;
//            UnityEngine.Debug.Log(A);
//            ECB.AddComponent(collisionEvent.BodyIndexA, A, new testingColliderTag());
//        }
//    }

//    public partial struct TriggerJob : ITriggerEventsJob
//    {
//        public EntityCommandBuffer.ParallelWriter ECB;
//        public void Execute(TriggerEvent triggerEvent)
//        {
//            Entity A = triggerEvent.EntityA;
//            Entity B = triggerEvent.EntityB;
//            UnityEngine.Debug.Log(A);
//            ECB.AddComponent(triggerEvent.BodyIndexA, A, new testingTriggerTag());
//        }
//    }

//    public struct testingColliderTag : IComponentData
//    {

//    }
//    public struct testingTriggerTag : IComponentData
//    {

//    }

//    public partial struct TestJob : IJobEntity
//    {
//        public void Execute()
//        {

//        }
//    }
//}


