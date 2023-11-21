﻿namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaName.Catalog);
        builder.Property(x => x.FirstName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(450).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
    }
}