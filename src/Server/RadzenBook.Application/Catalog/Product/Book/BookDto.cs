using RadzenBook.Application.Catalog.Author;
using RadzenBook.Application.Catalog.Category;
using RadzenBook.Application.Catalog.Genre;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product.Book;

public class BookDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string? Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductStatus Status { get; set; }
    public string? ISBN { get; set; }
    public string? Language { get; set; }
    public string? Translator { get; set; }
    public int PageCount { get; set; }
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public int Republish { get; set; }
    public CoverType CoverType { get; set; }
    public DateTime? PublishDate { get; set; }
    public CategoryDto Category { get; set; } = new();
    public AuthorDto? Author { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
    public List<ProductImageDto> Images { get; set; } = new();
}