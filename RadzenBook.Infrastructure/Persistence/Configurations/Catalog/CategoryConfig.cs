namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", SchemaName.Catalog);
        builder.Property(x => x.Title).HasMaxLength(450);
        builder.HasMany(x => x.Products)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId);
    }
}