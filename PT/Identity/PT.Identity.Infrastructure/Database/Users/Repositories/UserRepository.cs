using Microsoft.AspNetCore.Identity;
using PT.Common.Results;
using PT.Identity.Domain.Abstractions.Repositories;

namespace PT.Identity.Infrastructure.Database.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        { 
            _userManager = userManager;
        }

        public async Task<OperationResult> CreateAsync(Domain.User user, CancellationToken cancellationToken)
        {
            var userEntity = new User
            {
                Email = user.UserName,

            };

            var result = await _userManager.CreateAsync(userEntity, user.Password);

            if (result.Succeeded)
            {
                return OperationResult.FromSuccess();
            }
            else
            {
                return OperationResult.FromFailed(MapIdentityErrors(result.Errors));
            }
        }

        private static IReadOnlyCollection<Error> MapIdentityErrors(IEnumerable<IdentityError> errors)
            => errors.Select(e => new Error
            {
                Code = e.Code,
                Message = e.Description,
            }).ToList();
    }
}
