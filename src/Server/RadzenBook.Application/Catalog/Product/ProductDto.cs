using RadzenBook.Application.Catalog.Category;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product;

public class ProductDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string? Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; }
    public CategoryDto Category { get; set; } = new();
    public List<ProductImageDto> Images { get; set; } = new();
}