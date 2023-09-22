using Microsoft.EntityFrameworkCore;
using RadzenBook.Application.Common.Persistence.Repositories;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class PhotoRepository : BaseRepository<Domain.Catalog.Photo, string>, IPhotoRepository
{
    public PhotoRepository(DbContext context) : base(context)
    {
    }
}