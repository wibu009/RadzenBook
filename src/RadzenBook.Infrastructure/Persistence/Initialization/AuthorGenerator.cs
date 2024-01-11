namespace RadzenBook.Infrastructure.Persistence.Initialization;

public static class AuthorGenerator
{
    public static async Task<IEnumerable<Author>> GenerateAsync(DbContext context, int count = 20,
        string locale = "vi")
    {
        if (context.Set<Author>().Any()) return context.Set<Author>().ToList();

        var authors = new Faker<Author>(locale)
            .RuleFor(a => a.FullName, f => f.Person.FullName)
            .RuleFor(a => a.Alias, f => f.Person.UserName)
            .RuleFor(a => a.Biography, f => f.Lorem.Paragraph())
            .RuleFor(a => a.ImageUrl, f => f.Person.Avatar)
            .RuleFor(a => a.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(a => a.DateOfDeath, f => f.Date.Past())
            .Generate(count);
        await context.Set<Author>().AddRangeAsync(authors);

        return authors;
    }
}