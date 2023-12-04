namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PublisherAddressRepository : BaseRepository<PublisherAddress, Guid>, IPublisherAddressRepository
{
    protected PublisherAddressRepository(DbContext context) : base(context)
    {
    }
}