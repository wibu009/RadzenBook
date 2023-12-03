namespace RadzenBook.Domain.Catalog;

public class PublisherAddress : BaseEntity<Guid>
{
    public string AddressLine1 { get; set; } = default!;
    public string AddressLine2 { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public PublisherAddressType AddressType { get; set; }
    public Guid PublisherId { get; set; }
    public virtual Publisher Publisher { get; set; } = default!;
}