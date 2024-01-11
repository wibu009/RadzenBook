namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CustomerAddressConfig : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("CustomerAddresses", SchemaName.Catalog);
        builder.Property(x => x.AddressLine1).HasMaxLength(450);
        builder.Property(x => x.AddressLine2).HasMaxLength(450);
        builder.Property(x => x.City).HasMaxLength(200);
        builder.Property(x => x.State).HasMaxLength(200);
        builder.Property(x => x.Country).HasMaxLength(200);
        builder.Property(x => x.ZipCode).HasMaxLength(20);
        builder.Property(x => x.ConsigneeName).HasMaxLength(100);
        builder.Property(x => x.ConsigneePhoneNumber).HasMaxLength(20);
    }
}