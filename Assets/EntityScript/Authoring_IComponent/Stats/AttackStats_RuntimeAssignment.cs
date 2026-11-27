using Unity.Entities;
using Unity.Mathematics;

public struct AttackMovementIComponentData : IComponentData
{
    public float3 Direction;
    public float TimeExist;
}

public struct AttackTag : IComponentData
{
    public int ID;
}

public struct DestroyTag : IComponentData, IEnableableComponent
{

}