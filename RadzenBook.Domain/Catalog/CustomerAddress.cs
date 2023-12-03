namespace RadzenBook.Domain.Catalog;

public class CustomerAddress : BaseEntity<Guid>
{
    public string ConsigneeName { get; set; } = default!;
    public string ConsigneePhoneNumber { get; set; } = default!;
    public string AddressLine1 { get; set; } = default!;
    public string AddressLine2 { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public CustomerAddressType AddressType { get; set; }
    public bool IsDefault { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
}