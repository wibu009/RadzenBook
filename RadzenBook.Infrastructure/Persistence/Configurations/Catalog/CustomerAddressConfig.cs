namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CustomerAddressConfig : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("CustomerAddresses", SchemaName.Catalog);
        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CustomerId);
    }
}