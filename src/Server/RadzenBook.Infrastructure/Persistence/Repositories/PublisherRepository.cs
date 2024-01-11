namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PublisherRepository : BaseRepository<Publisher, Guid>, IPublisherRepository
{
    public PublisherRepository(DbContext context) : base(context)
    {
    }
}