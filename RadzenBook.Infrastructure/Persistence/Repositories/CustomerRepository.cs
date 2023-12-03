namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    protected CustomerRepository(DbContext context) : base(context)
    {
    }
}