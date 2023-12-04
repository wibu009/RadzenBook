namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class ProductDiscountConfig : IEntityTypeConfiguration<ProductDiscount>
{
    public void Configure(EntityTypeBuilder<ProductDiscount> builder)
    {
        builder.ToTable("ProductDiscounts", SchemaName.Catalog);
        builder.Property(x => x.Title).HasMaxLength(50);
        builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DiscountPercent).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DiscountType);
    }
}