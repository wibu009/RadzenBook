using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Sales;

public class OrderProgressConfig : IEntityTypeConfiguration<OrderProgress>
{
    public void Configure(EntityTypeBuilder<OrderProgress> builder)
    {
        builder.ToTable("OrderProgresses", SchemaName.Sales);
    }
}