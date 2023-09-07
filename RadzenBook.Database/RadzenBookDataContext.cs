using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RadzenBook.Entity;

namespace RadzenBook.Database;

public class RadzenBookDataContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public RadzenBookDataContext(DbContextOptions<RadzenBookDataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RadzenBookDataContext).Assembly);

        //config
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public virtual DbSet<Demo> Demos { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
    public virtual DbSet<AppRole> AppRoles { get; set; } = null!;
}