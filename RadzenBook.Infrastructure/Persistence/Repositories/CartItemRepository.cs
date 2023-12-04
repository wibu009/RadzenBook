namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CartItemRepository : BaseRepository<CartItem, Guid>, ICartItemRepository
{
    protected CartItemRepository(DbContext context) : base(context)
    {
    }
}