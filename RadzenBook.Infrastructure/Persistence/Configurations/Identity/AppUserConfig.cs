using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users", SchemaName.Identity);
        builder.HasMany(x => x.Addresses)
            .WithOne()
            .HasForeignKey(x => x.AppUserId)
            .IsRequired(false);
        builder.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.AppUser)
            .HasForeignKey(x => x.AppUserId);
    }
}