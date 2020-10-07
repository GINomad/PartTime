namespace PT.BuildingBlocks.Abstractions
{
    public interface IIdentifiable<TKey>
    {
        TKey Id { get; }
    }
}
