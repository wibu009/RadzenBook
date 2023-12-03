namespace RadzenBook.Domain.Catalog;

public class Cart : BaseEntity<Guid>
{
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
    public virtual ICollection<CartItem> Items { get; set; } = new HashSet<CartItem>();
}