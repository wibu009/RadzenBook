﻿using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Configurations.Sales;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaName.Sales);
        builder.Property(x => x.FirstName).HasMaxLength(200);
        builder.Property(x => x.LastName).HasMaxLength(200);
        builder.Property(x => x.Email).HasMaxLength(450);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.HasMany(x => x.Addresses)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId);
        builder.HasMany(x => x.Orders)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId);
        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId);
        builder.HasOne(x => x.Cart)
            .WithOne(x => x.Customer)
            .HasForeignKey<Cart>(x => x.CustomerId);
    }
}