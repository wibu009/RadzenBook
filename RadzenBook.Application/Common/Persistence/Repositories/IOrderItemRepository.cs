using RadzenBook.Domain.Sales;

namespace RadzenBook.Application.Common.Persistence.Repositories;

public interface IOrderItemRepository : IBaseRepository<OrderItem, Guid>
{
}