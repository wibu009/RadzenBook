namespace RadzenBook.Domain.Catalog;

public class Employee : BaseEntity<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Sex Gender { get; set; }
    public string EthnicOrigin { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinedDate { get; set; }
    public string PersonalPhoneNumber { get; set; } = default!;
    public string PersonalEmail { get; set; } = default!;
    public string Position { get; set; } = default!;
    public string IdentificationCode { get; set; } = default!;
    public DateTime IdentificationIssuedDate { get; set; }
    public string IdentificationIssuedPlace { get; set; } = default!;
    public string IdentificationIssuedCountry { get; set; } = default!;
    public string SocialInsuranceCode { get; set; } = default!;
    public string PersonalTaxCode { get; set; } = default!;
    public string BankAccountNumber { get; set; } = default!;
    public string BankAccountName { get; set; } = default!;
    public string BankName { get; set; } = default!;
    public string BankBranch { get; set; } = default!;
    public string EmergencyContactName { get; set; } = default!;
    public string EmergencyContactPhoneNumber { get; set; } = default!;
    public string EmergencyContactRelationship { get; set; } = default!;
    public decimal Salary { get; set; }
    public decimal Bonus { get; set; }
    public Guid UserId { get; set; }
    public ICollection<EmployeeAddress> Addresses { get; set; } = new HashSet<EmployeeAddress>();
}