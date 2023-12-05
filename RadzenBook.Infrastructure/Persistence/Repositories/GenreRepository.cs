namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class GenreRepository : BaseRepository<Genre, Guid>, IGenreRepository
{
    public GenreRepository(DbContext context) : base(context)
    {
    }
}