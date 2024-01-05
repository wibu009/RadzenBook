namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CustomerDiscountCodeConfig : IEntityTypeConfiguration<CustomerDiscountCode>
{
    public void Configure(EntityTypeBuilder<CustomerDiscountCode> builder)
    {
        builder.ToTable("CustomerDiscountCodes", SchemaName.Catalog);
    }
}