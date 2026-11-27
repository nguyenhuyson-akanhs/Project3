using Unity.Entities;
using UnityEngine;

public class ReceiveRequestCreateNewAttackSpawnPointAuthoring : MonoBehaviour
{

}
public class ReceiveRequestCreateNewAttackSpawnPointBaker : Baker<ReceiveRequestCreateNewAttackSpawnPointAuthoring>
{
    public override void Bake(ReceiveRequestCreateNewAttackSpawnPointAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new ReceiveRequestCreateNewAttackSpawnPointFlagData {ID = -1});
        SetComponentEnabled<ReceiveRequestCreateNewAttackSpawnPointFlagData>(entity, false);
    }
}
public struct ReceiveRequestCreateNewAttackSpawnPointFlagData : IComponentData, IEnableableComponent // -> icd + iec = flagdata
{
    public int ID;
}   