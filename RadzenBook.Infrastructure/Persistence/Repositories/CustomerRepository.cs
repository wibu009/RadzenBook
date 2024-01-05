namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(DbContext context) : base(context)
    {
    }
}