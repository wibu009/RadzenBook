namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(DbContext context) : base(context)
    {
    }
}