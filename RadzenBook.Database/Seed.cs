using Bogus;
using Microsoft.AspNetCore.Identity;
using RadzenBook.Common.Enums;
using RadzenBook.Entity;

namespace RadzenBook.Database;

public static class Seed
{
    public static async Task SeedData(this RadzenBookDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        // Create roles if they don't exist
        var roles = new List<string> { "manager", "employee", "customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
        }

        //check if there is any user in the database
        if (!userManager.Users.Any())
        {
            // Create a manager account
            var managerUser = new AppUser
            {
                UserName = "manager@example.com", Email = "manager@example.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            await userManager.CreateAsync(managerUser, "123456@Abc"); // Set a secure password
            await userManager.AddToRoleAsync(managerUser, "manager");

            // Create 3 employee accounts
            for (var i = 1; i <= 3; i++)
            {
                var employeeUser = new AppUser { UserName = $"employee{i}@example.com", Email = $"employee{i}@example.com", EmailConfirmed = true, PhoneNumberConfirmed = true };
                await userManager.CreateAsync(employeeUser, "123456@Abc"); // Set a secure password
                await userManager.AddToRoleAsync(employeeUser, "employee");
            }

            // Create 5 customer accounts
            for (var i = 1; i <= 5; i++)
            {
                var customerUser = new AppUser { UserName = $"customer{i}@example.com", Email = $"customer{i}@example.com", EmailConfirmed = true, PhoneNumberConfirmed = true };
                await userManager.CreateAsync(customerUser, "123456@Abc"); // Set a secure password
                await userManager.AddToRoleAsync(customerUser, "customer");
            }
        }
        
        // Create 20 demos
        if (!context.Demos.Any())
        {
            var demoFakers = new Faker<Demo>()
                .RuleFor(d => d.Name, f => f.Commerce.ProductName())
                .RuleFor(d => d.Description, f => f.Commerce.ProductDescription())
                .RuleFor(d => d.DemoEnum, f => f.PickRandom<DemoEnum>())
                .RuleFor(d => d.CreatedBy, "System")
                .RuleFor(d => d.CreatedAt, DateTime.UtcNow)
                .RuleFor(d => d.ModifiedBy, "System")
                .RuleFor(d => d.ModifiedAt, DateTime.UtcNow)
                .Generate(20);

            await context.Demos.AddRangeAsync(demoFakers);
        }
        
        // Create 20 addresses
        if (!context.Addresses.Any())
        {
            var addressFakers = new Faker<Address>()
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.Email, f => f.Internet.Email())
                .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber("0#########"))
                .RuleFor(a => a.FullName, f => f.Name.FullName())
                .RuleFor(a => a.CreatedBy, "System")
                .RuleFor(a => a.CreatedAt, DateTime.UtcNow)
                .RuleFor(a => a.ModifiedBy, "System")
                .RuleFor(a => a.ModifiedAt, DateTime.UtcNow)
                .Generate(20);

            await context.Addresses.AddRangeAsync(addressFakers);
        }

        await context.SaveChangesAsync();
    }
}