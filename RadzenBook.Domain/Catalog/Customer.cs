namespace RadzenBook.Domain.Catalog;

public class Customer : BaseEntity<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Sex Gender { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsGuest { get; set; }
    public Guid? UserId { get; set; }
    public Guid? CartId { get; set; }
    public virtual Cart? Cart { get; set; }
    public virtual ICollection<CustomerAddress> Addresses { get; set; } = new HashSet<CustomerAddress>();
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public virtual ICollection<CustomerDiscountCode> DiscountCodes { get; set; } = new HashSet<CustomerDiscountCode>();
}