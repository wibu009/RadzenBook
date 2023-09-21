namespace RadzenBook.Domain.Entities;

public class Address : BaseEntity<Guid>
{
    public string FullName { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? Email { get; set; } = default!;
}