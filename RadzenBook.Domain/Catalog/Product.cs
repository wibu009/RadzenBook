namespace RadzenBook.Domain.Catalog;

public class Product : BaseEntity<Guid>
{
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public CurrencyUnit Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = new();
    public virtual Book Book { get; set; } = new();
    public virtual ICollection<ProductDiscount> Discounts { get; set; } = new List<ProductDiscount>();
    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}