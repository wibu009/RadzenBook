namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product, Guid>, IProductRepository
{
    protected ProductRepository(DbContext context) : base(context)
    {
    }
}