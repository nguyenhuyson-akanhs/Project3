using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class PhysicAuthoring : MonoBehaviour
{
    
}

public class PhysicAuthoringBaker : Baker<PhysicAuthoring>
{
    public override void Bake(PhysicAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new PhysicTestTag());
        //AddComponent(entity, new PhysicsVelocity());
    }
}

public struct PhysicTestTag : IComponentData { }