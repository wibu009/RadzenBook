using Microsoft.EntityFrameworkCore;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;

namespace RadzenBook.Repository.Implements;

public class AddressRepository : BaseRepository<Address, Guid>, IAddressRepository
{
    public AddressRepository(DbContext context) : base(context)
    {
    }
}