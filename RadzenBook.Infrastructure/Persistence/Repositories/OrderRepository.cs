using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class OrderRepository : BaseRepository<Order, Guid>, IOrderRepository
{
    public OrderRepository(DbContext context) : base(context)
    {
    }
}