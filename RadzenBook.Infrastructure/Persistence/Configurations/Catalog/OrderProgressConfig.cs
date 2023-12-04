namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class OrderProgressConfig : IEntityTypeConfiguration<OrderProgress>
{
    public void Configure(EntityTypeBuilder<OrderProgress> builder)
    {
        builder.ToTable("OrderProgresses", SchemaName.Catalog);
    }
}