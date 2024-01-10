namespace RadzenBook.Infrastructure.Persistence.DataInitialization;

public static class GenresGenerator
{
    public static async Task<IEnumerable<Genre>> GenerateAsync(DbContext context, int count = 20,
        string locale = "vi")
    {
        if (context.Set<Genre>().Any()) return context.Set<Genre>().ToList();

        var genres = new Faker<Genre>(locale)
            .RuleFor(g => g.Name, f => f.Commerce.Department())
            .RuleFor(g => g.Description, f => f.Commerce.ProductDescription())
            .Generate(count);

        await context.Set<Genre>().AddRangeAsync(genres);
        return genres;
    }
}