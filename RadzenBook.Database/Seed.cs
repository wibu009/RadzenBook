using Bogus;
using RadzenBook.Common.Enums;
using RadzenBook.Entity;

namespace RadzenBook.Database;

public static class Seed
{
    public static async Task SeedData(this RadzenBookDataContext context)
    {
        if (context.Demos.Any()) return;
        var demoFaker = new Faker<Demo>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
            .RuleFor(x => x.DemoEnum, f => f.PickRandom<DemoEnum>());
        var demos = demoFaker.Generate(20);

        await context.Demos.AddRangeAsync(demos);
        await context.SaveChangesAsync();
    }
}