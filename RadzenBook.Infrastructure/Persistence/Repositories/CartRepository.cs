namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class CartRepository : BaseRepository<Cart, Guid>, ICartRepository
{
    protected CartRepository(DbContext context) : base(context)
    {
    }
}