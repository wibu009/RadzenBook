using RadzenBook.Infrastructure.Identity.Token;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Identity;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", SchemaName.Identity);
    }
}