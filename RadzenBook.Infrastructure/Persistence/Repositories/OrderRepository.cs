namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class OrderRepository : BaseRepository<Order, Guid>, IOrderRepository
{
    protected OrderRepository(DbContext context) : base(context)
    {
    }
}