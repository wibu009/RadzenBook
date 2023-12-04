namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class AuthorRepository : BaseRepository<Author, Guid>, IAuthorRepository
{
    protected AuthorRepository(DbContext context) : base(context)
    {
    }
}