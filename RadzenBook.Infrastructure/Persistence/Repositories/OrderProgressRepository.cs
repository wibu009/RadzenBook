using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class OrderProgressRepository : BaseRepository<OrderProgress, Guid>, IOrderProgressRepository
{
    public OrderProgressRepository(DbContext context) : base(context)
    {
    }
}