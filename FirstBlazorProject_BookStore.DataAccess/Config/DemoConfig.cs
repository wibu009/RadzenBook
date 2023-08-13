using FirstBlazorProject_BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstBlazorProject_BookStore.DataAccess.Config;

public class DemoConfig : IEntityTypeConfiguration<Demo>
{
    public void Configure(EntityTypeBuilder<Demo> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
    }
}