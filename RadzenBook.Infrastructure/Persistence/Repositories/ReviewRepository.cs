namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ReviewRepository : BaseRepository<Review, Guid>, IReviewRepository
{
    protected ReviewRepository(DbContext context) : base(context)
    {
    }
}