namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PublisherAddressRepository : BaseRepository<PublisherAddress, Guid>, IPublisherAddressRepository
{
    public PublisherAddressRepository(DbContext context) : base(context)
    {
    }
}