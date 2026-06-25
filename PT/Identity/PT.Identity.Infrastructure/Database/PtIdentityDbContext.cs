using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PT.Identity.Infrastructure.Database.Users;

namespace PT.Identity.Infrastructure.Database
{
    public class PtIdentityDbContext : IdentityDbContext<User>
    {
        public PtIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
