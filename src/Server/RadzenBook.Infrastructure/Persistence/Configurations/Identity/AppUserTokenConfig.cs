namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.ToTable("UserTokens", SchemaName.Identity);
    }
}