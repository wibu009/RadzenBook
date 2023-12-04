namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class BookRepository : BaseRepository<Book, Guid>, IBookRepository
{
    protected BookRepository(DbContext context) : base(context)
    {
    }
}