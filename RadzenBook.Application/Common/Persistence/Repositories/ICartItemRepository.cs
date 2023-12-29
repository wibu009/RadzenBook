using RadzenBook.Domain.Sales;

namespace RadzenBook.Application.Common.Persistence.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem, Guid>
{
}