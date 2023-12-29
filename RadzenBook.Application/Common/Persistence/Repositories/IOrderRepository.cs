using RadzenBook.Domain.Sales;

namespace RadzenBook.Application.Common.Persistence.Repositories;

public interface IOrderRepository : IBaseRepository<Order, Guid>
{
}