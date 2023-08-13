using FirstBlazorProject_BookStore.DataAccess.Config;
using FirstBlazorProject_BookStore.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorProject_BookStore.DataAccess.Context;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext()
    {

    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        //config
        modelBuilder.ApplyConfiguration(new DemoConfig());
    }

    public DbSet<Demo> Demos { get; set; }
}