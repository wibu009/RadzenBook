using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.CustomerAddress;

public class CustomerAddressDto
{
    public Guid Id { get; set; }
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
}