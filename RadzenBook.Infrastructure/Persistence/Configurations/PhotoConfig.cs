using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RadzenBook.Infrastructure.Persistence.Configurations;

public class PhotoConfig : IEntityTypeConfiguration<Domain.Entities.Photo>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Photo> builder)
    {
    }
}