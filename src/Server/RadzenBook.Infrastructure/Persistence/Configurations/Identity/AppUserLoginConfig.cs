namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable("UserLogins", SchemaName.Identity);
    }
}