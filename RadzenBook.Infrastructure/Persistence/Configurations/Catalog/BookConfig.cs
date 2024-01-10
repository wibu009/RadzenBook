namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", SchemaName.Catalog);
        builder.Property(x => x.ISBN).HasMaxLength(20);
        builder.Property(x => x.Language).HasMaxLength(50);
        builder.Property(x => x.Translator).HasMaxLength(200);
        builder.HasMany(x => x.Genres)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);
    }
}