namespace RadzenBook.Domain.Catalog;

public class CustomerAddress : BaseEntity<Guid>
{
    public string? ConsigneeName { get; set; }
    public string? ConsigneePhoneNumber { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public CustomerAddressType AddressType { get; set; }
    public bool IsDefault { get; set; }
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
}