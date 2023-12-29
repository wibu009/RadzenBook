using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Sales;

public class CartConfig : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts", SchemaName.Sales);
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Cart)
            .HasForeignKey(x => x.CartId);
    }
}