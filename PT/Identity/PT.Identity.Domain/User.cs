namespace PT.Identity.Domain
{
    public class User
    {
        public string? Id { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }
    }
}