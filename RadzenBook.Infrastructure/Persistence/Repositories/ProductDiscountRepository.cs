namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ProductDiscountRepository : BaseRepository<ProductDiscount, Guid>, IProductDiscountRepository
{
    protected ProductDiscountRepository(DbContext context) : base(context)
    {
    }
}