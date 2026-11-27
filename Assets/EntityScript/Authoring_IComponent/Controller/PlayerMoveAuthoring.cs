using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMoveAuthoring : MonoBehaviour
{
    public float MoveSpeed = 3f;
}

public class PlayerMoveAuthoringBaker : Baker<PlayerMoveAuthoring>
{
    public override void Bake(PlayerMoveAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new PlayerIComponentData
        { 
            MoveSpeed = authoring.MoveSpeed,
        });
        AddComponent(entity, new InputsIComponentData
        {
        });
    }
}

public struct PlayerIComponentData : IComponentData
{
    public float MoveSpeed;
}

public struct InputsIComponentData : IComponentData
{
    public float2 MoveDirection;
}