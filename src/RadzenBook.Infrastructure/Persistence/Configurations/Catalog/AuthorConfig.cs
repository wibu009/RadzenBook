namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors", SchemaName.Catalog);
        builder.Property(x => x.FullName).HasMaxLength(450);
        builder.HasMany(x => x.Books)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
    }
}