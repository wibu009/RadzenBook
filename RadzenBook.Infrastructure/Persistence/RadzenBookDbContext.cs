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

    public virtual DbSet<Demo> Demos { get; set; } = null!;

    //Identity
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    //Catalog
    public virtual DbSet<Author> Authors { get; set; } = null!;
    public virtual DbSet<Book> Books { get; set; } = null!;
    public virtual DbSet<BookGenre> BookGenres { get; set; } = null!;
    public virtual DbSet<Cart> Carts { get; set; } = null!;
    public virtual DbSet<CartItem> CartItems { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; } = null!;
    public virtual DbSet<CustomerDiscountCode> CustomerDiscountCodes { get; set; } = null!;
    public virtual DbSet<DiscountCode> DiscountCodes { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<EmployeeAddress> EmployeeAddresses { get; set; } = null!;
    public virtual DbSet<Genre> Genres { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
    public virtual DbSet<OrderProgress> OrderProgresses { get; set; } = null!;
    public virtual DbSet<Payment> Payments { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; } = null!;
    public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
    public virtual DbSet<Publisher> Publishers { get; set; } = null!;
    public virtual DbSet<PublisherAddress> PublisherAddresses { get; set; } = null!;
    public virtual DbSet<Review> Reviews { get; set; } = null!;
}