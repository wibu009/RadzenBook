using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CartRepository : BaseRepository<Cart, Guid>, ICartRepository
{
    public CartRepository(DbContext context) : base(context)
    {
    }
}