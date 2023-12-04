namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class GenreRepository : BaseRepository<Genre, Guid>, IGenreRepository
{
    protected GenreRepository(DbContext context) : base(context)
    {
    }
}