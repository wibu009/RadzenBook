using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Sales;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", SchemaName.Sales);
        builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.ShippingCost).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Tax).HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.Payments)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);
        builder.HasMany(x => x.Progresses)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);
    }
}