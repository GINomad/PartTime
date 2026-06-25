using PT.Common.Results;
using PT.Identity.Client.Api.Dtos;
using PT.Identity.Domain;

namespace PT.Identity.Application.Abstractions.DataReaders
{
    public interface IUserDR
    {
        Task<OperationResult> LoginAsync(UserLoginDto dto);

        Task<OperationResult<IReadOnlyCollection<Role>>> GetRolesAsync(UserLoginDto dto);
    }
}
