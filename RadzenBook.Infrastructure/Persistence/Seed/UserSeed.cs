using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        //check if there is any user in the database
        if (!userManager.Users.Any())
        {
            // Create a manager account
            var managerUser = new AppUser
            {
                UserName = "manager",
                Email = "manager@example.com",
                DisplayName = "Manager",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            await userManager.CreateAsync(managerUser, "123456@Abc"); // Set a secure password
            await userManager.AddToRoleAsync(managerUser, RoleName.Manager);

            // Create 3 employee accounts
            for (var i = 1; i <= 3; i++)
            {
                var employeeUser = new AppUser
                {
                    UserName = $"employee{i}",
                    Email = $"employee{i}@example.com",
                    DisplayName = $"Employee {i}",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(employeeUser, "123456@Abc"); // Set a secure password
                await userManager.AddToRoleAsync(employeeUser, RoleName.Employee);
            }

            // Create 5 customer accounts
            for (var i = 1; i <= 5; i++)
            {
                var customerUser = new AppUser
                {
                    UserName = $"customer{i}",
                    Email = $"customer{i}@example.com",
                    DisplayName = $"Customer {i}",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(customerUser, "123456@Abc"); // Set a secure password
                await userManager.AddToRoleAsync(customerUser, RoleName.Customer);
            }
        }
    }
}