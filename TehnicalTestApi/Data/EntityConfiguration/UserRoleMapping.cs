using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnicalTestApi.Entities;

namespace TechnicalTestApi.Data.EntityConfiguration;

internal class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles")
            .HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}