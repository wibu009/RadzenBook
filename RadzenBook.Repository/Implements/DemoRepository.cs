using FirstBlazorProject_BookStore.DataAccess;
using FirstBlazorProject_BookStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorProject_BookStore.Repository.Implements;

public class DemoRepository: BaseRepository<Entity.Demo, Guid>, IDemoRepository
{
    protected DemoRepository(DbContext context) : base(context)
    {
    }
}