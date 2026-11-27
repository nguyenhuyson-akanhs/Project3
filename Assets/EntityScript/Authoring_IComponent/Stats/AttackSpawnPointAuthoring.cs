using Unity.Entities;
using UnityEngine;

public class AttackSpawnPointAuthoring : MonoBehaviour
{
    
}

public class AttackSpawnPointAuthoringBaker : Baker<AttackSpawnPointAuthoring>
{
    public override void Bake(AttackSpawnPointAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new AttackSpawnPointTag { });
        AddComponent(entity, new DestroyTag { });
        SetComponentEnabled<DestroyTag>(entity, false);
    }
}
