using Unity.Burst;
using Unity.Entities;

partial struct ReadingWayPointISystem_testing : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //WayPointsBlobIComponentData data = SystemAPI.GetSingleton<WayPointsBlobIComponentData>();
        //ref WayPointsBlobData blobData = ref data.WayPointsBlobRef.Value;
        //for(int i = 0;i < blobData.WayPointsArray.Length; i++)
        //{
        //    UnityEngine.Debug.Log(blobData.WayPointsArray[i].Position);
        //}
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
