using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

public struct AttackSpawnPointDataIBufferElement : IBufferElementData
{
    public Entity EntityPrefab;
}

public class AttackSpawnPointDataDynamicBufferAuthoring : MonoBehaviour
{
    public List<GameObject> AttackSpawnPoint;
}

public class AttackSpawnPointDataDynamicBufferBaker : Baker<AttackSpawnPointDataDynamicBufferAuthoring>
{
    public override void Bake(AttackSpawnPointDataDynamicBufferAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        DynamicBuffer<AttackSpawnPointDataIBufferElement> buffer = AddBuffer<AttackSpawnPointDataIBufferElement>(entity);
        foreach (var g in authoring.AttackSpawnPoint)
        {
            buffer.Add(new AttackSpawnPointDataIBufferElement
            {
                EntityPrefab = GetEntity(g, TransformUsageFlags.Dynamic)
            });
        }
    }
}