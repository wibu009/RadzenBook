namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CustomerDiscountCodeRepository : BaseRepository<CustomerDiscountCode, Guid>,
    ICustomerDiscountCodeRepository
{
    public CustomerDiscountCodeRepository(DbContext context) : base(context)
    {
    }
}