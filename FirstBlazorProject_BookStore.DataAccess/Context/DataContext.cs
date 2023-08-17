﻿using FirstBlazorProject_BookStore.DataAccess.Config;
using FirstBlazorProject_BookStore.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorProject_BookStore.DataAccess.Context;

public class DataContext : IdentityDbContext<AppUser, AppRole, Guid>
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

    public virtual DbSet<Demo> Demos { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<AppRole> AppRoles { get; set; }
}