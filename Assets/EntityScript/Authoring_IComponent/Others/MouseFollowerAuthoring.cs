using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MouseFollowerAuthoring : MonoBehaviour
{
    
}

public class MouseFollowerAuthoringBaker : Baker<MouseFollowerAuthoring>
{
    public override void Bake(MouseFollowerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new MouseFollowerSingletonIComponentData { });
    }
}

public struct MouseFollowerSingletonIComponentData : IComponentData { }

public partial class MouseFollowerSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<MouseFollowerSingletonIComponentData>();
    }
    protected override void OnUpdate()
    {
        foreach(var(mouseSingleton,localTrs) in SystemAPI.Query<RefRO<MouseFollowerSingletonIComponentData>, RefRW<LocalTransform>>())
        {
            float3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -Camera.main.transform.position.z;
            float3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            localTrs.ValueRW.Position = mouseWorldPos;
        }
    }
}