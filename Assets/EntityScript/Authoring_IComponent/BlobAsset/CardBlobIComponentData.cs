using Unity.Entities;

public enum TypeOfCard
{

}

public struct Card
{
    public Entity EntityPrefab;
}
public struct CardDataBase
{
    public BlobArray<Card> CardData;
}

public struct CardBlobIComponentData : IComponentData
{
    public BlobAssetReference<CardDataBase> BlobRef; // next target, build blobref for attack, this one is for attack point and special(later)
}
