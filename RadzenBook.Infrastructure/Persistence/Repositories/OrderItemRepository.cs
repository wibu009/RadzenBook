namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class OrderItemRepository : BaseRepository<OrderItem, Guid>, IOrderItemRepository
{
    protected OrderItemRepository(DbContext context) : base(context)
    {
    }
}