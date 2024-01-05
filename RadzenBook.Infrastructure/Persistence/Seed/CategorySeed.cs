using Bogus;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedCategories(RadzenBookDbContext context)
    {
        // Create 20 categories
        if (!context.Categories.Any())
        {
            var categoryFakers = new Faker<Category>()
                .RuleFor(c => c.Title, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
                .RuleFor(c => c.CreatedBy, "System")
                .RuleFor(c => c.CreatedAt, DateTime.UtcNow)
                .RuleFor(c => c.ModifiedBy, "System")
                .RuleFor(c => c.ModifiedAt, DateTime.UtcNow)
                .Generate(20);

            await context.Categories.AddRangeAsync(categoryFakers);
        }
    }
}