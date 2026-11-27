using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class AttackTriggerAuthoring : MonoBehaviour
{
    [Header("AttackPrefab")]
    public GameObject NormalAttackPrefab;
    public GameObject FireAttackPrefab;

    [Header("AttackSpawnPointPrefab")]
    public float RotatingSpeed;
    public float Radius;
    public GameObject NormalAttackSpawnPointPrefab;
    public GameObject FireAttackSpawnPointPrefab;
}

public class AttackTriggerAuthoringBaker : Baker<AttackTriggerAuthoring>
{
    public override void Bake(AttackTriggerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new AttackTriggerSingletonIComponentData
        {
            NormalAttackPrefab = GetEntity(authoring.NormalAttackPrefab, TransformUsageFlags.Dynamic),
            FireAttackPrefab = GetEntity(authoring.FireAttackPrefab, TransformUsageFlags.Dynamic),
        });
        AddComponent(entity, new AttackSpawnPointPrefabsSingletonIComponentData
        {

            NormalAttackSpawnPointPrefab = GetEntity(authoring.NormalAttackSpawnPointPrefab, TransformUsageFlags.Dynamic),
            FireAttackSpawnPointPrefab = GetEntity(authoring.FireAttackSpawnPointPrefab, TransformUsageFlags.Dynamic),
        });
        AddComponent(entity, new AttackSpawnPointStatsSingletonIComponentData
        {
            RotatingSpeed = authoring.RotatingSpeed,
            Radius = authoring.Radius,
        });
    }
}

public struct AttackTriggerSingletonIComponentData : IComponentData
{
    public Entity NormalAttackPrefab;
    public Entity FireAttackPrefab;
}

public struct AttackSpawnPointPrefabsSingletonIComponentData : IComponentData
{
    public Entity NormalAttackSpawnPointPrefab;
    public Entity FireAttackSpawnPointPrefab;
}

public struct AttackSpawnPointStatsSingletonIComponentData : IComponentData 
{
    public float RotatingSpeed;
    public float Radius;
}

public struct AttackSpawnPointTag : IComponentData
{
    public int IDAttack;
}
