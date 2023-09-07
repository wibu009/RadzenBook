using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Entity.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorProject_BookStore.DataAccess.Context;

public class RadzenBookDataContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public RadzenBookDataContext()
    {

    }

    public RadzenBookDataContext(DbContextOptions<RadzenBookDataContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RadzenBookDataContext).Assembly);

        //config
        modelBuilder.ApplyConfiguration(new DemoConfig());
    }

    public virtual DbSet<Demo> Demos { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
    public virtual DbSet<AppRole> AppRoles { get; set; } = null!;
}