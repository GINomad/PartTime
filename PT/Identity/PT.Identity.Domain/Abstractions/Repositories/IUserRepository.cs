using PT.Common.Results;

namespace PT.Identity.Domain.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateAsync(User user, CancellationToken cancellationToken);
    }
}
