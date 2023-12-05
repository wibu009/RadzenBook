namespace RadzenBook.Domain.Catalog;

public class Employee : BaseEntity<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public string? EthnicOrigin { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinedDate { get; set; }
    public string? PersonalPhoneNumber { get; set; }
    public string? PersonalEmail { get; set; }
    public string? Position { get; set; }
    public string? IdentificationCode { get; set; }
    public DateTime IdentificationIssuedDate { get; set; }
    public string? IdentificationIssuedPlace { get; set; }
    public string? IdentificationIssuedCountry { get; set; }
    public string? SocialInsuranceCode { get; set; }
    public string? PersonalTaxCode { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhoneNumber { get; set; }
    public string? EmergencyContactRelationship { get; set; }
    public decimal Salary { get; set; }
    public decimal Bonus { get; set; }
    public Guid UserId { get; set; }
    public ICollection<EmployeeAddress> Addresses { get; set; } = new HashSet<EmployeeAddress>();
}