//using Unity.Burst;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Physics;
//using Unity.Physics.Extensions;

//public partial struct PhysicTestingISystem : ISystem
//{
//    [BurstCompile]
//    public void OnCreate(ref SystemState state)
//    {
//        state.RequireForUpdate<PhysicTestTag>();
//    }

//    public void OnUpdate(ref SystemState state)
//    {
//        foreach (var (velocity, mass, entity) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<PhysicsMass>>().WithAll<PhysicTestTag>().WithEntityAccess())
//        {
//            //velocity.ValueRW.Linear += new float3(0, 1f, 0);
//            //velocity.ValueRW.ApplyAngularImpulse(mass.ValueRO,new float3(3f,0,0));
//            if (UnityEngine.Input.GetMouseButtonDown(0))
//            {
//                velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, new float3(3f, 0, 0));
//            }
//            if (math.lengthsq(velocity.ValueRW.Linear) < 0.01f)
//            {
//                velocity.ValueRW.Linear = float3.zero;
//            }
//        }
//    }

//    [BurstCompile]
//    public void OnDestroy(ref SystemState state)
//    {

//    }
//}
