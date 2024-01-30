using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnicalTestApi.Entities;

namespace TechnicalTestApi.Data.EntityConfiguration;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(x => x.Id);

        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.UserName).IsRequired();

        builder.Property(x => x.FirstName).HasMaxLength(255);
        builder.Property(x => x.LastName).HasMaxLength(255);
        builder.Property(x => x.Title).HasMaxLength(50);
        builder.Property(x => x.Address).HasMaxLength(255);
        builder.Property(x => x.Bio).HasMaxLength(255);
        builder.Property(x => x.Country).HasMaxLength(255);
        builder.Property(x => x.Image).HasMaxLength(1000);
        builder.Property(x => x.RefreshToken).HasMaxLength(255);

        builder.HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}