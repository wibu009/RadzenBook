namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class DemoConfig : IEntityTypeConfiguration<Demo>
{
    public void Configure(EntityTypeBuilder<Demo> builder)
    {
        builder.ToTable("Demos", SchemaName.Default);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);
    }
}