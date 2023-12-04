namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee, Guid>, IEmployeeRepository
{
    protected EmployeeRepository(DbContext context) : base(context)
    {
    }
}