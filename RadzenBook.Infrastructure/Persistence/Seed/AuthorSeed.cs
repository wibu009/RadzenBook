using Bogus;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedAuthors(RadzenBookDbContext context)
    {
        // Create 20 authors
        if (!context.Authors.Any())
        {
            var authorFakers = new Faker<Author>()
                .RuleFor(a => a.FullName, f => f.Person.FullName)
                .RuleFor(a => a.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(a => a.Alias, f => f.Person.UserName)
                .RuleFor(a => a.DateOfDeath, f => f.Person.DateOfBirth)
                .RuleFor(a => a.Biography, f => f.Lorem.Paragraph())
                .RuleFor(a => a.CreatedBy, "System")
                .RuleFor(a => a.CreatedAt, DateTime.UtcNow)
                .RuleFor(a => a.ModifiedBy, "System")
                .RuleFor(a => a.ModifiedAt, DateTime.UtcNow)
                .Generate(20);

            await context.Authors.AddRangeAsync(authorFakers);
        }
    }
}