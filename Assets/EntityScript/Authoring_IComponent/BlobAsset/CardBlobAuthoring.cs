using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class CardBlobAuthoring : MonoBehaviour
{
    [System.Serializable]
    public struct CardData
    {
        public GameObject GameObjectPrefab;
    }
    public CardData[] CardArray; // <- id = index
}

public class CardBlobBaker : Baker<CardBlobAuthoring>
{
    public override void Bake(CardBlobAuthoring authoring)
    {
        if (authoring.CardArray == null || authoring.CardArray.Length == 0) return;

        using var builder = new BlobBuilder(Allocator.Temp); // create builder
        ref CardDataBase cardDataBaseRoot = ref builder.ConstructRoot<CardDataBase>(); // create root from builder
        BlobBuilderArray<Card> cardArrayInBuild = builder.Allocate(ref cardDataBaseRoot.CardData, authoring.CardArray.Length); // array in build
        for (int i = 0; i < authoring.CardArray.Length; i++)
        {
            cardArrayInBuild[i].EntityPrefab = GetEntity(authoring.CardArray[i].GameObjectPrefab, TransformUsageFlags.Dynamic);
        }
        BlobAssetReference<CardDataBase> blobRef = builder.CreateBlobAssetReference<CardDataBase>(Allocator.Persistent);

        AddBlobAsset(ref blobRef, out var hash); // add to subscene //TODO: check unitydoc this one

        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new CardBlobIComponentData { BlobRef = blobRef }); // must bug here
    }
}
