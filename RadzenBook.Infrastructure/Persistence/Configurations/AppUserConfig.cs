using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Configurations;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasMany(x => x.Addresses)
            .WithOne()
            .HasForeignKey(x => x.AppUserId)
            .IsRequired(false);

        builder.HasMany(x => x.Photos)
            .WithOne()
            .HasForeignKey(x => x.AppUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}