namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class BookRepository : BaseRepository<Book, Guid>, IBookRepository
{
    public BookRepository(DbContext context) : base(context)
    {
    }
}