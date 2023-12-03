namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class EmployeeAddressConfig : IEntityTypeConfiguration<EmployeeAddress>
{
    public void Configure(EntityTypeBuilder<EmployeeAddress> builder)
    {
        builder.ToTable("EmployeeAddresses", SchemaName.Catalog);
        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.EmployeeId);
    }
}