namespace RadzenBook.Domain.Catalog;

public class Product : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal ImportPrice { get; set; }
    public decimal SellPrice { get; set; }
    public ProductStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = new();
    public virtual Book Book { get; set; } = new();
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}