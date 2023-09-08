namespace PT.Identity.Domain
{
    public class Role
    {
        public string? Id { get; set; } = default!;

        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        public string? NormalizedName { get; set; }
    }
}
