using PT.Identity.Client.Api.Dtos;
using PT.Identity.Domain;

namespace PT.Identity.Application.Mappers
{
    public static class UserMapper
    {
        public static User Map(UserRegistrationDto dto)
            => new()
            {
                UserName = dto.UserName,
                Password = dto.Password,
            };
    }
}
