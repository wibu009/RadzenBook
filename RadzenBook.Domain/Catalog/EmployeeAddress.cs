namespace RadzenBook.Domain.Catalog;

public class EmployeeAddress : BaseEntity<Guid>
{
    public string AddressLine1 { get; set; } = default!;
    public string AddressLine2 { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public EmployeeAddressType AddressType { get; set; }
    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = default!;
}