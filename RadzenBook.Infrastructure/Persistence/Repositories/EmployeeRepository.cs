namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee, Guid>, IEmployeeRepository
{
    public EmployeeRepository(DbContext context) : base(context)
    {
    }
}