using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct EnemyDataBufferElement : IBufferElementData
{
    public Entity EntityPrefab;
    public float NormalHP;
    public float NormalDefense;
    public float NormalMoveSpeed;
    public float NormalStrength;
    public float SpawnRate;
}

public class EnemyDataDynamicBufferAuthoring : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyData
    {
        public GameObject Prefab;
        public float NormalHP;
        public float NormalDefense;
        public float NormalMoveSpeed;
        public float NormalStrength;
        public float SpawnRate;
    }

    public List<EnemyData> Enemy;
}

public class EnemyDataDynamicBufferBaker : Baker<EnemyDataDynamicBufferAuthoring>
{
    public override void Bake(EnemyDataDynamicBufferAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        DynamicBuffer<EnemyDataBufferElement> buffer = AddBuffer<EnemyDataBufferElement>(entity);
        foreach (var e in authoring.Enemy)
        {
            buffer.Add(new EnemyDataBufferElement
            {
                EntityPrefab = GetEntity(e.Prefab, TransformUsageFlags.Dynamic),
                NormalHP = e.NormalHP,
                NormalDefense = e.NormalDefense,
                NormalMoveSpeed =  e.NormalMoveSpeed,
                NormalStrength = e.NormalStrength,
                SpawnRate = e.SpawnRate,
            });
        }
    }
}