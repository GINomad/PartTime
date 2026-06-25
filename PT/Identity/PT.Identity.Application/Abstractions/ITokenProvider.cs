using PT.Common.Results;
using System.Security.Claims;

namespace PT.Identity.Application.Abstractions
{
    public interface ITokenProvider
    {
        Task<OperationResult<string>> CreateTokenAsync(IEnumerable<Claim> claims);
    }
}
