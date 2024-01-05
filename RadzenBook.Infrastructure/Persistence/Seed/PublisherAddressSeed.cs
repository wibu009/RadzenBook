using Bogus;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedPublishersAddress(RadzenBookDbContext context)
    {
        // Create 20 categories
        if (!context.PublisherAddresses.Any())
        {
            var publisherAddressFakers = new Faker<PublisherAddress>()

               .RuleFor(p => p.AddressLine1, f => f.Address.StreetAddress())
               .RuleFor(p => p.AddressLine2, f => f.Address.SecondaryAddress())
               .RuleFor(p => p.City, f => f.Address.City())
               .RuleFor(p => p.State, f => f.Address.State())
               .RuleFor(p => p.Country, f => f.Address.Country())
               .RuleFor(p => p.ZipCode, f => f.Address.ZipCode())
               .RuleFor(p => p.AddressType, f => f.PickRandom<PublisherAddressType>())
               .RuleFor(p => p.PublisherId, f => f.Random.Guid())
               .RuleFor(p => p.CreatedBy, "System")
               .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
               .RuleFor(p => p.ModifiedBy, "System")
               .RuleFor(p => p.ModifiedAt, DateTime.UtcNow)
               .Generate(20);

            await context.PublisherAddresses.AddRangeAsync(publisherAddressFakers);
        }
    }
}
