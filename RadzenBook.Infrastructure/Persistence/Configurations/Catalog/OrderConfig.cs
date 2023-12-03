namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", SchemaName.Catalog);
        builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.ShippingCost).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.Tax).HasColumnType("decimal(18,2)").IsRequired();
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