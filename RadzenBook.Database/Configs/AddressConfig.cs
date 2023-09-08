using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RadzenBook.Entity;

namespace RadzenBook.Database.Configs;

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Street).HasMaxLength(100).IsRequired();
        builder.Property(x => x.City).HasMaxLength(50).IsRequired();
        builder.Property(x => x.State).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ZipCode).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Country).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(50);
    }
}