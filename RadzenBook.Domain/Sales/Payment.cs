namespace RadzenBook.Domain.Sales;

public class Payment : BaseEntity<Guid>
{
    public decimal Amount { get; set; }
    public DateTime? Paid { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order? Order { get; set; }
}