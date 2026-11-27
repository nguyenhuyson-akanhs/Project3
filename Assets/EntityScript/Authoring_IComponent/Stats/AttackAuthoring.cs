using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class AttackAuthoring : MonoBehaviour
{

}

public class AttackAuthoringBaker : Baker<AttackAuthoring>
{
    public override void Bake(AttackAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new AttackMovementIComponentData { });
        AddComponent(entity, new AttackTag { });
        AddComponent(entity, new DestroyTag { });
        SetComponentEnabled<DestroyTag>(entity, false);
    }
}