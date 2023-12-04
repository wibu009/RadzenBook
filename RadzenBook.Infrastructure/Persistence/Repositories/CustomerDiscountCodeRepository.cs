namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CustomerDiscountCodeRepository : BaseRepository<CustomerDiscountCode, Guid>,
    ICustomerDiscountCodeRepository
{
    protected CustomerDiscountCodeRepository(DbContext context) : base(context)
    {
    }
}