using Unity.Entities;
using UnityEngine;

public partial class RecieveRequestCreateNewCardSystemBase : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        CardInteracter.CardInteracted += EventListenner;
    }
    protected override void OnUpdate()
    {

    }
    protected override void OnDestroy()
    {
        CardInteracter.CardInteracted -= EventListenner;
        base.OnDestroy();
    }

    private void EventListenner(int id)
    {
        // f it </3 f = flag, e = entity
        foreach(var (f,e) in SystemAPI.Query<RefRO<ReceiveRequestCreateNewAttackSpawnPointFlagData>>()
                                      .WithEntityAccess()
                                      .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            Debug.Log("CardInteracted event is listenned");
            EntityManager.SetComponentEnabled<ReceiveRequestCreateNewAttackSpawnPointFlagData>(e, true);
            EntityManager.SetComponentData(e,new ReceiveRequestCreateNewAttackSpawnPointFlagData { ID = id });
        }
    }
}
