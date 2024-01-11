namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("UserClaims", SchemaName.Identity);
    }
}