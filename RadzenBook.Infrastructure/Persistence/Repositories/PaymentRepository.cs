using RadzenBook.Domain.Sales;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PaymentRepository : BaseRepository<Payment, Guid>, IPaymentRepository
{
    public PaymentRepository(DbContext context) : base(context)
    {
    }
}