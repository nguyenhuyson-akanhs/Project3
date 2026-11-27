using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHp;
}

public class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new EnemyTag { });
        AddComponent(entity, new DestroyTag { });
        SetComponentEnabled<DestroyTag>(entity, false);
        AddComponent(entity, new EnemyStats
        {
            MaxHP = authoring.MaxHP,
            CurrentHp = authoring.CurrentHp,
        });
    }
}

public struct EnemyStats : IComponentData
{
    public int MaxHP;
    public int CurrentHp;

}
