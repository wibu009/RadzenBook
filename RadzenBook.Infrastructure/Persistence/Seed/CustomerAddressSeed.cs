using Bogus;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedCustomerAddress(RadzenBookDbContext context)
    {
        // Create 20 CustomerAddress
        if (!context.CustomerAddresses.Any())
        {
            var demoFakers = new Faker<CustomerAddress>(locale:"vi")
                .RuleFor(d => d.ConsigneeName, f => f.Person.FullName)
                .RuleFor(d => d.ConsigneePhoneNumber, f => f.Person.Phone)
                .RuleFor(d => d.AddressLine1, f => f.Address.FullAddress())
                .RuleFor(d => d.AddressLine2, f => f.Address.FullAddress())
                .RuleFor(d => d.City, f => f.Address.City())
                .RuleFor(d => d.State, f => f.Address.State())
                .RuleFor(d => d.Country, f => f.Address.Country())
                .RuleFor(d => d.ZipCode, f => f.Address.ZipCode())
                .RuleFor(d => d.IsDefault, f => true)
                .Generate(20);

            await context.CustomerAddresses.AddRangeAsync(demoFakers);
        }
    }
}