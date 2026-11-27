using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct AttackTriggerISystem : ISystem
{
    public ComponentLookup<LocalTransform> LookUpLocalTrs;
    public ComponentLookup<AttackSpawnPointTag> LookUpASPTag;
    public EntityQuery EQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AttackTriggerSingletonIComponentData>();
        state.RequireForUpdate<PlayerFollowerSingletonIComponentData>();
        state.RequireForUpdate<MouseFollowerSingletonIComponentData>();

        EQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<AttackSpawnPointTag>()
                                                       .Build(ref state);

        LookUpLocalTrs = state.GetComponentLookup<LocalTransform>(true);
        LookUpASPTag = state.GetComponentLookup<AttackSpawnPointTag>(true);
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (f, e) in SystemAPI.Query<RefRO<ReceiveRequestCreateNewAttackSpawnPointFlagData>>()
                              .WithEntityAccess())
        {
            AddNewElementToASPArray(ref state, f.ValueRO);
            state.EntityManager.SetComponentEnabled<ReceiveRequestCreateNewAttackSpawnPointFlagData>(e, false);
            state.EntityManager.SetComponentData(e, new ReceiveRequestCreateNewAttackSpawnPointFlagData { ID = -1 });
            return;
        }

        if (!UnityEngine.Input.GetMouseButtonDown(0)) return;

        if (!SystemAPI.HasSingleton<PlayerFollowerSingletonIComponentData>()
            || !SystemAPI.HasSingleton<MouseFollowerSingletonIComponentData>()) return;

        LookUpLocalTrs.Update(ref state); // update lookup
        LookUpASPTag.Update(ref state);

        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                                                   .CreateCommandBuffer(state.WorldUnmanaged);

        NativeArray<Entity> ASPArray = EQuery.ToEntityArray(Allocator.TempJob);
        if (ASPArray.Length == 0)
        {
            ASPArray.Dispose();
            return;
        }

        AttackTriggerJob job = new AttackTriggerJob
        {
            ECB = ecb.AsParallelWriter(),
            ASPArray = ASPArray,
            LocalTrs = LookUpLocalTrs,
            ASPTag = LookUpASPTag,
            CurrentPlayerPos = SystemAPI.GetComponent<LocalTransform>(
                    SystemAPI.GetSingletonEntity<PlayerFollowerSingletonIComponentData>())
                    .Position,
            CurrentMousePos = SystemAPI.GetComponent<LocalTransform>(
                    SystemAPI.GetSingletonEntity<MouseFollowerSingletonIComponentData>())
                    .Position,
            AttackDataDB = SystemAPI.GetSingletonBuffer<AttackDataIBufferElement>(true),
        };
        JobHandle jobHandle = job.ScheduleParallel(1, 64, state.Dependency);

        ASPArray.Dispose(jobHandle);

        state.Dependency = jobHandle;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
    public void AddNewElementToASPArray(ref SystemState state, ReceiveRequestCreateNewAttackSpawnPointFlagData f)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                                           .CreateCommandBuffer(state.WorldUnmanaged);

        DynamicBuffer<AttackSpawnPointDataIBufferElement> data = SystemAPI.GetSingletonBuffer<AttackSpawnPointDataIBufferElement>(true);
        Entity entityPrefab = data[f.ID].EntityPrefab;

        Entity entity = ecb.Instantiate(entityPrefab); // save entity never use blobasset
        ecb.SetComponent(entity, new AttackSpawnPointTag { IDAttack = f.ID });
    }
}
[BurstCompile]
public partial struct AttackTriggerJob : IJobFor
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public NativeArray<Entity> ASPArray;
    [ReadOnly] public ComponentLookup<LocalTransform> LocalTrs;
    [ReadOnly] public ComponentLookup<AttackSpawnPointTag> ASPTag;
    [ReadOnly] public DynamicBuffer<AttackDataIBufferElement> AttackDataDB;
    public float3 CurrentPlayerPos;
    public float3 CurrentMousePos;
    [BurstCompile]
    public void Execute(int index)
    {
        foreach (Entity e in ASPArray)
        {
            if (e == Entity.Null) break;
            Entity entity = ECB.Instantiate(index, AttackDataDB[ASPTag[e].IDAttack].EntityPrefab);
            float3 localTrs = LocalTrs[e].Position;
            ECB.SetComponent(index, entity, LocalTransform.FromPositionRotationScale
            (
                localTrs,
                quaternion.identity,
                AttackDataDB[ASPTag[e].IDAttack].NormalScale
            ));
            ECB.SetComponent(index, entity, new AttackMovementIComponentData 
            {
                Direction = math.normalize(CurrentMousePos - localTrs),
                TimeExist = AttackDataDB[ASPTag[e].IDAttack].TimeExist,
            });
            ECB.SetComponent(index, entity, new AttackTag { ID = ASPTag[e].IDAttack });
        }
    }
}
/* Ví dụ khi làm game Roguelite:
 * Khi đạt ngưỡng exp cần thiết -> popup các card cấp(UI + Button/EventTrigger)
 * Trong button/eventtrigger tạo hàm phát event đến systembase -> gán input cho 1 icomponentdata
 * ví dụ selectedCard : icomponentdata{int idCard,int versionCard} id tương đương loại card, version tương đương cấp độ card (?v)
 * khi thực hiện click vào 1 card -> phát event đến 1 systembase -> addcomponentdata cho 1 entity(VD: gameControllerData)
 * -> trong isystem, sử dụng getsingleton<co(mponentdata>, phát hiện thực hiện tạo thêm 1 attackpoint mới
 * sử dụng boldAssest để lưu dữ liệu tĩnh id(loại card) - stats(card)?
 */


