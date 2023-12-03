using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RadzenBook.Infrastructure.Identity;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Persistence.Configurations;

namespace RadzenBook.Infrastructure.Persistence;

public class RadzenBookDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public RadzenBookDbContext(DbContextOptions<RadzenBookDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasDefaultSchema(SchemaName.Default);
    }


    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public virtual DbSet<Demo> Demos { get; set; } = null!;
    public virtual DbSet<Domain.Catalog.ProductImage> Photos { get; set; } = null!;
}