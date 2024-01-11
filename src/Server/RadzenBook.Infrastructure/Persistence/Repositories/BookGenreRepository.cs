namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class BookGenreRepository : BaseRepository<BookGenre, Guid>, IBookGenreRepository
{
    public BookGenreRepository(DbContext context) : base(context)
    {
    }
}