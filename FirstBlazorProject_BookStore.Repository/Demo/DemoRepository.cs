using FirstBlazorProject_BookStore.Repository.Base;

namespace FirstBlazorProject_BookStore.Repository.Demo;

public class DemoRepository: BaseRepository<DataAccess.Entities.Demo, Guid>, IDemoRepository
{
    protected DemoRepository(DataAccess.Context.DataContext dataContext) : base(dataContext)
    {
    }
}