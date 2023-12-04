﻿namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class DiscountCodeConfig : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.ToTable("DiscountCodes", SchemaName.Catalog);
        builder.Property(x => x.Code);
        builder.Property(x => x.DiscountPercentage).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MinimumPurchaseAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.StartDate);
        builder.HasMany(x => x.CustomerDiscountCodes)
            .WithOne(x => x.DiscountCode)
            .HasForeignKey(x => x.DiscountCodeId);
    }
}