using RadzenBook.Application.Catalog.Product.Book;
using RadzenBook.Application.Catalog.Publisher.Address;

namespace RadzenBook.Application.Catalog.Publisher;

public class PublisherDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = default!;
    public virtual List<BookDto> Books { get; set; } = new();
    public virtual List<PublisherAddressDto> Addresses { get; set; } = new();
}