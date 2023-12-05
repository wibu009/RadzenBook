namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CartItemRepository : BaseRepository<CartItem, Guid>, ICartItemRepository
{
    public CartItemRepository(DbContext context) : base(context)
    {
    }
}