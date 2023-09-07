namespace RadzenBook.Entity;

public class Address : BaseEntity<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public Guid? UserId { get; set; }
    public virtual AppUser? User { get; set; } = default!;
}