namespace RadzenBook.Domain.Catalog;

public class Cart : BaseEntity<Guid>
{
    public decimal TotalPrice { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
    public virtual ICollection<CartItem> Items { get; set; } = new HashSet<CartItem>();
}