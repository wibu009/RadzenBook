using RadzenBook.Application.Common.Persistence.Repositories;
using RadzenBook.Domain.Catalog;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class DemoRepository: BaseRepository<Demo, Guid>, IDemoRepository
{
    public DemoRepository(DbContext context) : base(context)
    {
    }
}