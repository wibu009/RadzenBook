namespace RadzenBook.Domain.Catalog;

public class PaymentMethod : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}