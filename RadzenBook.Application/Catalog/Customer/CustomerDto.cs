using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Customer;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsGuest { get; set; }
    public Guid? UserId { get; set; }
    public Guid? CartId { get; set; }
}