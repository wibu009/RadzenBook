using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Sales;

public class CustomerDiscountCodeConfig : IEntityTypeConfiguration<CustomerDiscountCode>
{
    public void Configure(EntityTypeBuilder<CustomerDiscountCode> builder)
    {
        builder.ToTable("CustomerDiscountCodes", SchemaName.Sales);
    }
}