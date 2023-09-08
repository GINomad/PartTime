using PT.Common.Commands;
using PT.Common.Results;
using PT.Identity.Application.Abstractions;
using PT.Identity.Application.Abstractions.DataReaders;
using PT.Identity.Client.Api.Dtos;
using System.Security.Claims;

namespace PT.Identity.Application.Commands.Login
{
    public record LoginUserCommand(UserLoginDto Dto)
        : ICommand<OperationResult<string>>
    {
    }

    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, OperationResult<string>>
    {
        private readonly IUserDR _userDr;

        private readonly ITokenProvider _tokenProvider;

        public LoginUserCommandHandler(
            IUserDR userDr,
            ITokenProvider tokenProvider)
        {
            _userDr = userDr;
            _tokenProvider = tokenProvider;
        }

        public async Task<OperationResult<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _userDr.LoginAsync(request.Dto);

            if (!loginResult.Succeeded)
            {
                return loginResult!.TransformTo<string>();
            }

            var claims = await GetClaims(request.Dto);

            return await _tokenProvider.CreateTokenAsync(claims);
        }

        private async Task<List<Claim>> GetClaims(UserLoginDto dto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.UserName!)
            };
            var rolesResult = await _userDr.GetRolesAsync(dto);

            if (rolesResult.Succeeded)
            {
                foreach (var role in rolesResult.Result!)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name!));
                }
            }
            
            return claims;
        }
    }
}
