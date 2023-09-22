using Microsoft.EntityFrameworkCore;
using RadzenBook.Application.Common.Persistence.Repositories;
using RadzenBook.Domain.Catalog;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class AddressRepository : BaseRepository<Address, Guid>, IAddressRepository
{
    public AddressRepository(DbContext context) : base(context)
    {
    }
}