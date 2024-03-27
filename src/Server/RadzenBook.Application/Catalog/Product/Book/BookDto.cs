using RadzenBook.Application.Catalog.Author;
using RadzenBook.Application.Catalog.Genre;
using RadzenBook.Application.Catalog.Publisher;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product.Book;

public class BookDto : ProductDto
{
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
    public AuthorDto Author { get; set; } = default!;
    public PublisherDto Publisher { get; set; } = default!;
    public List<GenreDto> Genres { get; set; } = new();
}