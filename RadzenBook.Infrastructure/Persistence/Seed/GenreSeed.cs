using Bogus;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public partial class Seed
{
    private static async Task SeedGenres(RadzenBookDbContext context)
    {
        // Create 20 genres
        if (!context.Genres.Any())
        {
            var genreFakers = new Faker<Genre>()
                .RuleFor(g => g.Name, f => f.Commerce.Department())
                .RuleFor(g => g.Description, f => f.Commerce.ProductDescription())
                .RuleFor(g => g.CreatedBy, "System")
                .RuleFor(g => g.CreatedAt, DateTime.UtcNow)
                .RuleFor(g => g.ModifiedBy, "System")
                .RuleFor(g => g.ModifiedAt, DateTime.UtcNow)
                .Generate(20);

            await context.Genres.AddRangeAsync(genreFakers);
        }
    }
}