using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct AttackDataIBufferElement : IBufferElementData
{
    public Entity EntityPrefab;
    public float NormalScale;
    public float NormalSpeed;
    public float NormalRotateSpeed;
    public float TimeExist;
}

public class AttackDataDynamicBufferAuthoring : MonoBehaviour
{
    [System.Serializable]
    public struct AttackData
    {
        public GameObject AttackPrefab;
        public float NormalScale;
        public float NormalSpeed;
        public float NormalRotateSpeed;
        public float TimeExist;
    }
    public List<AttackData> Attack;
}

public class AttackDataDynamicBufferBaker : Baker<AttackDataDynamicBufferAuthoring>
{
    public override void Bake(AttackDataDynamicBufferAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        DynamicBuffer<AttackDataIBufferElement> buffer = AddBuffer<AttackDataIBufferElement>(entity);
        foreach (var g in authoring.Attack)
        {
            buffer.Add(new AttackDataIBufferElement
            {
                EntityPrefab = GetEntity(g.AttackPrefab, TransformUsageFlags.Dynamic),
                NormalScale = g.NormalScale,
                NormalSpeed = g.NormalSpeed,
                NormalRotateSpeed = g.NormalRotateSpeed,
                TimeExist = g.TimeExist
            });
        }
    }
}

public struct SharedTest : ISharedComponentData, IEquatable<SharedTest>
{
    public int ShareID;
    public bool Equals(SharedTest other)
    {
        return other.ShareID == ShareID;
    }
    public override int GetHashCode()
    {
        return ShareID.GetHashCode();
    }
}
