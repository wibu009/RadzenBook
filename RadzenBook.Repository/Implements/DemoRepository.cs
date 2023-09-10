using Microsoft.EntityFrameworkCore;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;

namespace RadzenBook.Repository.Implements;

public class DemoRepository: BaseRepository<Demo, Guid>, IDemoRepository
{
    public DemoRepository(DbContext context) : base(context)
    {
    }
}