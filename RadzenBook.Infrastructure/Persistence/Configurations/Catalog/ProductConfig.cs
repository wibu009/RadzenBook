namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", SchemaName.Catalog);
        builder.Property(x => x.ImportPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.SalePrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);
        builder.HasOne(x => x.Book)
            .WithOne(x => x.Product)
            .HasForeignKey<Book>(x => x.ProductId);
        builder.HasMany(x => x.Images)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.CartItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.Discounts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
    }
}