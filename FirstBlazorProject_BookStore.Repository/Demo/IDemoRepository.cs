using FirstBlazorProject_BookStore.Repository.Base;

namespace FirstBlazorProject_BookStore.Repository.Demo;

public interface IDemoRepository : IBaseRepository<DataAccess.Entities.Demo, Guid>
{
    
}