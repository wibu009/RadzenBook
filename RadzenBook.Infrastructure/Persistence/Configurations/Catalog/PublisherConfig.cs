namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class PublisherConfig : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.ToTable("Publishers", SchemaName.Catalog);
        builder.Property(x => x.Name).HasMaxLength(450).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(450).IsRequired();
        builder.HasMany(x => x.Books)
            .WithOne(x => x.Publisher)
            .HasForeignKey(x => x.PublisherId);
        builder.HasMany(x => x.Addresses)
            .WithOne(x => x.Publisher)
            .HasForeignKey(x => x.PublisherId);
    }
}