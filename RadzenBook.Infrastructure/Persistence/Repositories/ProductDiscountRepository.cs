using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ProductDiscountRepository : BaseRepository<ProductDiscount, Guid>, IProductDiscountRepository
{
    public ProductDiscountRepository(DbContext context) : base(context)
    {
    }
}