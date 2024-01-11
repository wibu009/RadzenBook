namespace RadzenBook.Infrastructure.Persistence.Initialization;

public static class CategoryGenerator
{
    public static async Task<IEnumerable<Category>> GenerateAsync(DbContext context, int count = 20,
        string locale = "vi")
    {
        if (context.Set<Category>().Any()) return context.Set<Category>().ToList();

        var categories = new Faker<Category>(locale)
            .RuleFor(c => c.Title, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
            .Generate(count);

        await context.Set<Category>().AddRangeAsync(categories);

        return categories;
    }
}