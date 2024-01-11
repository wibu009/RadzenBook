using RadzenBook.Infrastructure.Identity.Role;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppRoleConfig : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("Roles", SchemaName.Identity);
    }
}