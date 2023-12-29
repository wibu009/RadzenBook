using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Sales;

public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", SchemaName.Sales);
    }
}