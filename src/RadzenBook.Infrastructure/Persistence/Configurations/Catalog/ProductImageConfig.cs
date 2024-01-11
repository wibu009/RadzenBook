namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages", SchemaName.Catalog);
    }
}