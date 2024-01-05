namespace RadzenBook.Infrastructure.Persistence.Configurations.Catalog;

public class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees", SchemaName.Catalog);
        builder.Property(x => x.FirstName).HasMaxLength(200);
        builder.Property(x => x.LastName).HasMaxLength(200);
        builder.Property(x => x.Position).HasMaxLength(200);
        builder.Property(x => x.PersonalEmail).HasMaxLength(450);
        builder.Property(x => x.PersonalPhoneNumber).HasMaxLength(20);
        builder.Property(x => x.IdentificationCode).HasMaxLength(50);
        builder.Property(x => x.IdentificationIssuedPlace).HasMaxLength(450);
        builder.Property(x => x.IdentificationIssuedDate);
        builder.Property(x => x.IdentificationIssuedCountry).HasMaxLength(200);
        builder.Property(x => x.SocialInsuranceCode).HasMaxLength(50);
        builder.Property(x => x.PersonalTaxCode).HasMaxLength(50);
        builder.Property(x => x.EmergencyContactName).HasMaxLength(200);
        builder.Property(x => x.EmergencyContactPhoneNumber).HasMaxLength(20);
        builder.Property(x => x.EmergencyContactRelationship).HasMaxLength(200);
        builder.Property(x => x.BankAccountName).HasMaxLength(200);
        builder.Property(x => x.BankAccountNumber).HasMaxLength(50);
        builder.Property(x => x.BankName).HasMaxLength(200);
        builder.Property(x => x.BankBranch).HasMaxLength(200);
        builder.Property(x => x.Salary).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Bonus).HasColumnType("decimal(18,2)");
    }
}