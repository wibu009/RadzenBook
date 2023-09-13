using Microsoft.EntityFrameworkCore;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;

namespace RadzenBook.Repository.Implements;

public class PhotoRepository : BaseRepository<Photo, string>, IPhotoRepository
{
    public PhotoRepository(DbContext context) : base(context)
    {
    }
}