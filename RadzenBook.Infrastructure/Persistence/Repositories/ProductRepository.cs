namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(DbContext context) : base(context)
    {
    }
}