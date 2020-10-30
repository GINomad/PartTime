namespace PT.Identity.Abstractions
{
    public interface IUser
    {
        string Id { get; }
        int? ClientId { get; }
    }
}
