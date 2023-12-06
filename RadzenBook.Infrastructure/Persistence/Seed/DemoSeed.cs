using Bogus;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedDemos(RadzenBookDbContext context)
    {
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
    }
}