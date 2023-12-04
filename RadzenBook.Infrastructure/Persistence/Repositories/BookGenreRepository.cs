namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class BookGenreRepository : BaseRepository<BookGenre, Guid>, IBookGenreRepository
{
    protected BookGenreRepository(DbContext context) : base(context)
    {
    }
}