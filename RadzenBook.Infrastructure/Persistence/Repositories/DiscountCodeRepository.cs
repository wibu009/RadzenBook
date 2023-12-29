using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class DiscountCodeRepository : BaseRepository<DiscountCode, Guid>, IDiscountCodeRepository
{
    public DiscountCodeRepository(DbContext context) : base(context)
    {
    }
}