using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PT.Core.Client.Domain;
using PT.Identity;

namespace PT.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasMaxLength(100);
            builder.Property(p => p.LastName).HasMaxLength(100);
            builder.Property(p => p.UserId).IsRequired();
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }
}
