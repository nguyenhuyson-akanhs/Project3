
using Unity.Entities;
using Unity.Mathematics;

public struct WayPoints
{
    public float3 Position;
}
public struct WayPointsBlobData
{
    public BlobArray<WayPoints> WayPointsArray;
}
public struct WayPointsBlobIComponentData : IComponentData
{
    public BlobAssetReference<WayPointsBlobData> WayPointsBlobRef;
}