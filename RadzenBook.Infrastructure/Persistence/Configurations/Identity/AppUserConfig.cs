using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users", SchemaName.Identity);
        builder.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.AppUser)
            .HasForeignKey(x => x.AppUserId);
        builder.HasOne(x => x.Customer)
            .WithOne()
            .HasForeignKey<Customer>(x => x.UserId);
        builder.HasOne(x => x.Employee)
            .WithOne()
            .HasForeignKey<Employee>(x => x.UserId);
    }
}