namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.ToTable("UserRoles", SchemaName.Identity);
    }
}