using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RadzenBook.Domain.Catalog;
using RadzenBook.Infrastructure.Identity;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence;

public class RadzenBookDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public RadzenBookDbContext(DbContextOptions<RadzenBookDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
    

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public virtual DbSet<Demo> Demos { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<Domain.Catalog.Photo> Photos { get; set; } = null!;
}