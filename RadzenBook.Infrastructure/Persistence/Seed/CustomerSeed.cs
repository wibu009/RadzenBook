using Bogus;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedCustomer(RadzenBookDbContext context)
    {
        // Create 20 Customer
        if (!context.Customers.Any())
        {
            var demoFakers = new Faker<Customer>(locale:"vi")
                .RuleFor(d => d.FirstName, f => f.Person.FirstName)
                .RuleFor(d => d.LastName, f => f.Person.LastName)
                .RuleFor(d => d.Email, f => f.Person.Email)
                .RuleFor(d => d.PhoneNumber, f => f.Person.Phone)
                .RuleFor(d => d.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(d => d.Email, f => f.Person.Email)
                .RuleFor(d => d.IsGuest, f => true)
                .Generate(20);

            await context.Customers.AddRangeAsync(demoFakers);
        }
    }
}