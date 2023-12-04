namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CartItemConfig : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", SchemaName.Catalog);
        builder.Property(x => x.Quantity);
    }
}