using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class OrderItemRepository : BaseRepository<OrderItem, Guid>, IOrderItemRepository
{
    public OrderItemRepository(DbContext context) : base(context)
    {
    }
}