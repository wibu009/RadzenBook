namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ProductImageRepository : BaseRepository<ProductImage, string>, IProductImageRepository
{
    public ProductImageRepository(DbContext context) : base(context)
    {
    }
}