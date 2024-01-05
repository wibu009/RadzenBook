namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CartConfig : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts", SchemaName.Catalog);
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Cart)
            .HasForeignKey(x => x.CartId);
    }
}