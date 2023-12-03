namespace RadzenBook.Domain.Catalog;

public class Publisher : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    public virtual ICollection<PublisherAddress> Addresses { get; set; } = new HashSet<PublisherAddress>();
}