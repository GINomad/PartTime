using PT.Common.Commands;
using PT.Common.Results;
using PT.Identity.Application.Mappers;
using PT.Identity.Client.Api.Dtos;
using PT.Identity.Domain.Abstractions.Repositories;

namespace PT.Identity.Application.Commands.Register
{
    public record RegisterUserCommand(UserRegistrationDto Dto) : ICommand<OperationResult>;

    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = UserMapper.Map(request.Dto);

            var result = await _userRepository.CreateAsync(user, cancellationToken);

            return result;
        }
    }
}
