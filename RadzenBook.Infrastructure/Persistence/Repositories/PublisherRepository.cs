namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PublisherRepository : BaseRepository<Publisher, Guid>, IPublisherRepository
{
    protected PublisherRepository(DbContext context) : base(context)
    {
    }
}