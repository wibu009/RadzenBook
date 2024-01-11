namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres", SchemaName.Catalog);
        builder.Property(x => x.Name).HasMaxLength(450);
        builder.HasMany(x => x.Books)
            .WithOne(x => x.Genre)
            .HasForeignKey(x => x.GenreId);
    }
}