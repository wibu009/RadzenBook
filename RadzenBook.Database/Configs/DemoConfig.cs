using FirstBlazorProject_BookStore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstBlazorProject_BookStore.DataAccess.Configs;

public class DemoConfig : IEntityTypeConfiguration<Demo>
{
    public void Configure(EntityTypeBuilder<Demo> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);
    }
}