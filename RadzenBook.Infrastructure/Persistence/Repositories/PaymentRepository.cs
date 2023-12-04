namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PaymentRepository : BaseRepository<Payment, Guid>, IPaymentRepository
{
    protected PaymentRepository(DbContext context) : base(context)
    {
    }
}