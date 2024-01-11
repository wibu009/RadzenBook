namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class EmployeeAddressRepository : BaseRepository<EmployeeAddress, Guid>, IEmployeeAddressRepository
{
    public EmployeeAddressRepository(DbContext context) : base(context)
    {
    }
}