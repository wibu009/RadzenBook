using FirstBlazorProject_BookStore.Repository.Interfaces;

namespace FirstBlazorProject_BookStore.Repository.Implements;

public class DemoRepository: BaseRepository<Entity.Demo, Guid>, IDemoRepository
{
    protected DemoRepository(DataAccess.Context.RadzenBookDataContext radzenBookDataContext) : base(radzenBookDataContext)
    {
    }
}