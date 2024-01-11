using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Initialization;

public static class UserGenerator
{
    public static async Task<IEnumerable<AppUser>> GenerateAsync(UserManager<AppUser> userManager)
    {
        if (userManager.Users.Any()) return userManager.Users.ToList();

        #region Create a manager account

        var managerUser = new Faker<AppUser>("vi")
            .RuleFor(u => u.UserName, f => "manager")
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.DisplayName, f => f.Person.FullName)
            .RuleFor(u => u.PhoneNumber, f => f.Person.Phone)
            .RuleFor(u => u.EmailConfirmed, true)
            .RuleFor(u => u.PhoneNumberConfirmed, true)
            .Generate();
        await userManager.CreateAsync(managerUser, "123456@Abc"); // Set a secure password
        await userManager.AddToRoleAsync(managerUser, RoleName.Manager);

        #endregion

        #region Create 5 employee accounts

        for (var i = 1; i <= 5; i++)
        {
            var employeeUser = new Faker<AppUser>("vi")
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.DisplayName, f => f.Person.FullName)
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone)
                .RuleFor(u => u.EmailConfirmed, true)
                .RuleFor(u => u.PhoneNumberConfirmed, true)
                .Generate();
            await userManager.CreateAsync(employeeUser, "123456@Abc"); // Set a secure password
            await userManager.AddToRoleAsync(employeeUser, RoleName.Employee);
        }

        #endregion

        #region Create 10 customer accounts

        for (var i = 1; i <= 10; i++)
        {
            var customerUser = new Faker<AppUser>("vi")
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.DisplayName, f => f.Name.FullName())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.EmailConfirmed, true)
                .RuleFor(u => u.PhoneNumberConfirmed, true)
                .Generate();
            await userManager.CreateAsync(customerUser, "123456@Abc"); // Set a secure password
            await userManager.AddToRoleAsync(customerUser, RoleName.Customer);
        }

        #endregion

        return userManager.Users.ToList();
    }
}