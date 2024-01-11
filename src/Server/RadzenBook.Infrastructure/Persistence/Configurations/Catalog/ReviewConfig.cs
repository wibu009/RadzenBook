namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", SchemaName.Catalog);
        builder.Property(x => x.Title).HasMaxLength(450);
    }
}