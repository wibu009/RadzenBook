namespace RadzenBook.Domain.Catalog;

public class Customer : BaseEntity<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Sex Gender { get; set; }
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; } = default!;
    public bool IsGuest { get; set; }
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
    public virtual Cart Cart { get; set; } = default!;
    public virtual ICollection<CustomerAddress> Addresses { get; set; } = new HashSet<CustomerAddress>();
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}