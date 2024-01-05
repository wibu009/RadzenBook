using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Publisher;

public class PublisherDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}

