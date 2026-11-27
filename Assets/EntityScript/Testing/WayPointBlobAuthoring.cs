using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
//using static WayPointBlobAuthoring;

public class WayPointBlobAuthoring : MonoBehaviour
{
    [System.Serializable]
    public struct WayPointsAuthoring
    {
        public float3 Position;
    }
    public WayPointsAuthoring[] WayPointsAuthorings;
}
public class WayPointBlobAuthoringBaker : Baker<WayPointBlobAuthoring>
{
    public override void Bake(WayPointBlobAuthoring authoring)
    {
        using var builder = new BlobBuilder(Allocator.Temp); // create builder
        ref WayPointsBlobData WPBDRoot = ref builder.ConstructRoot<WayPointsBlobData>(); // create root from builder
        BlobBuilderArray<WayPoints> builderArray = builder.Allocate(ref WPBDRoot.WayPointsArray, authoring.WayPointsAuthorings.Length); // create blobAsset in building(from builder, this one type array)
        // this using ref to change value in root
        for (int i = 0; i < authoring.WayPointsAuthorings.Length; i++)
        {
            builderArray[i].Position = authoring.WayPointsAuthorings[i].Position; // crate value in blobAsset
        }

        BlobAssetReference<WayPointsBlobData> blobRef = builder.CreateBlobAssetReference<WayPointsBlobData>(Allocator.Persistent); // create real blobAsset
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new WayPointsBlobIComponentData { WayPointsBlobRef = blobRef }); // create IComponenetData for access,
                                                                                              // access blobAsset throuh BlobAssetReference in IComponentData
    }
}
