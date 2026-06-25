namespace PT.Identity.Client.Api.Dtos
{
    public class UserRegistrationDto
    {
        public UserRegistrationDto()
        {
            UserName = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
