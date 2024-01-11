namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CustomerAddressRepository : BaseRepository<CustomerAddress, Guid>, ICustomerAddressRepository
{
    public CustomerAddressRepository(DbContext context) : base(context)
    {
    }
}