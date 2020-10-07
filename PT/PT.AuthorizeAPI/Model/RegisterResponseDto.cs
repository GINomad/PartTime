using PT.Identity;

namespace PT.AuthorizeAPI.Model
{
    public class RegisterResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public RegisterResponseDto(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email;
        }
    }
}
