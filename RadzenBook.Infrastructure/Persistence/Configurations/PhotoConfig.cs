using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RadzenBook.Infrastructure.Persistence.Configurations;

public class PhotoConfig : IEntityTypeConfiguration<Domain.Catalog.Photo>
{
    public void Configure(EntityTypeBuilder<Domain.Catalog.Photo> builder)
    {
    }
}