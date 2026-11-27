using Unity.Entities;
using UnityEngine;

public class PlayerFollowerAuthoring : MonoBehaviour
{
    
}

public class PlayerFollowerAuthoringBaker : Baker<PlayerFollowerAuthoring>
{
    public override void Bake(PlayerFollowerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new PlayerFollowerSingletonIComponentData { });
    }
}

public struct PlayerFollowerSingletonIComponentData : IComponentData { }

