namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class AuthorRepository : BaseRepository<Author, Guid>, IAuthorRepository
{
    public AuthorRepository(DbContext context) : base(context)
    {
    }
}