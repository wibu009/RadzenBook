﻿namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class EmployeeAddressConfig : IEntityTypeConfiguration<EmployeeAddress>
{
    public void Configure(EntityTypeBuilder<EmployeeAddress> builder)
    {
        builder.ToTable("EmployeeAddresses", SchemaName.Catalog);
        builder.Property(x => x.AddressLine1).HasMaxLength(450);
        builder.Property(x => x.AddressLine2).HasMaxLength(450);
        builder.Property(x => x.City).HasMaxLength(200);
        builder.Property(x => x.State).HasMaxLength(200);
        builder.Property(x => x.Country).HasMaxLength(200);
        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.EmployeeId);
    }
}