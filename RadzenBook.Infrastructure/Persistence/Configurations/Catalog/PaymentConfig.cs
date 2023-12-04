namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class PaymentConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", SchemaName.Catalog);
        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
    }
}