namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class BookGenreConfig : IEntityTypeConfiguration<BookGenre>
{
    public void Configure(EntityTypeBuilder<BookGenre> builder)
    {
        builder.ToTable("BookGenres", SchemaName.Catalog);
    }
}