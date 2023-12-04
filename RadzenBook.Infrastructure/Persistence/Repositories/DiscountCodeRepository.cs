namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class DiscountCodeRepository : BaseRepository<DiscountCode, Guid>, IDiscountCodeRepository
{
    protected DiscountCodeRepository(DbContext context) : base(context)
    {
    }
}