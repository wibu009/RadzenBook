namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", SchemaName.Catalog);
    }
}