namespace RadzenBook.Domain.Catalog;

public class EmployeeAddress : BaseEntity<Guid>
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public EmployeeAddressType AddressType { get; set; }
    public Guid? EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
}