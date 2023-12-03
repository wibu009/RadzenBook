namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class PublisherAddressConfig : IEntityTypeConfiguration<PublisherAddress>
{
    public void Configure(EntityTypeBuilder<PublisherAddress> builder)
    {
        builder.ToTable("PublisherAddresses", SchemaName.Catalog);
        builder.Property(x => x.AddressLine1).HasMaxLength(450).IsRequired();
        builder.Property(x => x.AddressLine2).HasMaxLength(450);
        builder.Property(x => x.City).HasMaxLength(100).IsRequired();
        builder.Property(x => x.State).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ZipCode).HasMaxLength(20).IsRequired();
    }
}