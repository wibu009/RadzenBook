using FirstBlazorProject_BookStore.Repository.Base;

namespace FirstBlazorProject_BookStore.Repository.Demo;

public class DemoRepository: BaseRepository<Entity.Demo, Guid>, IDemoRepository
{
    protected DemoRepository(DataAccess.Context.BookStoreDataContext bookStoreDataContext) : base(bookStoreDataContext)
    {
    }
}