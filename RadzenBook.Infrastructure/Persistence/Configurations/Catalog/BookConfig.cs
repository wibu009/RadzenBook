namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", SchemaName.Catalog);
        builder.Property(x => x.ISBN).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(400).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Language).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Translator).HasMaxLength(200);
        builder.Property(x => x.PageCount).IsRequired();
        builder.Property(x => x.Weight).IsRequired();
        builder.Property(x => x.Width).IsRequired();
        builder.Property(x => x.Height).IsRequired();
        builder.Property(x => x.Depth).IsRequired();
        builder.Property(x => x.Republish).IsRequired();
        builder.HasMany(x => x.Genres)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);
    }
}