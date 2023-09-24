namespace RadzenBook.Application.Catalog.Address.Query;

public class AddressDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public Guid AppUserId { get; set; } = default!;
}