namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppRoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable("RoleClaims", SchemaName.Identity);
    }
}