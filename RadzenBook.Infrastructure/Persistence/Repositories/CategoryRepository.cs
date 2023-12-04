namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
{
    protected CategoryRepository(DbContext context) : base(context)
    {
    }
}