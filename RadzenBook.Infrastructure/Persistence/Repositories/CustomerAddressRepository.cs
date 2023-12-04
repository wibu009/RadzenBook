namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CustomerAddressRepository : BaseRepository<CustomerAddress, Guid>, ICustomerAddressRepository
{
    protected CustomerAddressRepository(DbContext context) : base(context)
    {
    }
}