namespace RadzenBook.Infrastructure.Persistence.DataInitialization;

public static class DemoGenerator
{
    public static async Task<IEnumerable<Demo>> GenerateAsync(DbContext context, int count = 20,
        string locale = "vi")
    {
        if (context.Set<Demo>().Any()) return context.Set<Demo>().ToList();

        var demos = new Faker<Demo>(locale)
            .RuleFor(d => d.Name, f => f.Commerce.ProductName())
            .RuleFor(d => d.Description, f => f.Commerce.ProductDescription())
            .RuleFor(d => d.DemoEnum, f => f.PickRandom<DemoEnum>())
            .Generate(count);

        await context.Set<Demo>().AddRangeAsync(demos);

        return demos;
    }
}