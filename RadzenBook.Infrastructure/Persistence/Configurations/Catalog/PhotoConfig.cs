namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class PhotoConfig : IEntityTypeConfiguration<Domain.Catalog.Photo>
{
    public void Configure(EntityTypeBuilder<Domain.Catalog.Photo> builder)
    {
        builder.ToTable("Photos", SchemaName.Catalog);
    }
}