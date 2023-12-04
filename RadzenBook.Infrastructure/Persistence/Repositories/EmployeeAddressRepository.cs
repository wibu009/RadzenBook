namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class EmployeeAddressRepository : BaseRepository<EmployeeAddress, Guid>, IEmployeeAddressRepository
{
    protected EmployeeAddressRepository(DbContext context) : base(context)
    {
    }
}