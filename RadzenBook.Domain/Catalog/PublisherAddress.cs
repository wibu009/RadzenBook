namespace RadzenBook.Domain.Catalog;

public class PublisherAddress : BaseEntity<Guid>
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public PublisherAddressType AddressType { get; set; }
    public Guid PublisherId { get; set; }
    public virtual Publisher? Publisher { get; set; }
}