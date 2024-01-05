using Bogus;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedPublishers(RadzenBookDbContext context)
    {
        // Create 20 categories
        if (!context.Publishers.Any())
        {
            var publisherFakers = new Faker<Publisher>()
               .RuleFor(p => p.Name, f => f.Commerce.Department())
               .RuleFor(g => g.Description, f => f.Commerce.ProductDescription())
               .RuleFor(p => p.Email, f => f.Internet.Email())
               .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
               .RuleFor(p => p .CreatedBy, "System")
               .RuleFor(p => p .CreatedAt, DateTime.UtcNow)
               .RuleFor(p => p .ModifiedBy, "System")
               .RuleFor(p => p.ModifiedAt, DateTime.UtcNow)
               .Generate(20);

            await context.Publishers.AddRangeAsync(publisherFakers);
        }
    }
}
