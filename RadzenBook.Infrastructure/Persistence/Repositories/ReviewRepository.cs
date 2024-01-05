namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class ReviewRepository : BaseRepository<Review, Guid>, IReviewRepository
{
    public ReviewRepository(DbContext context) : base(context)
    {
    }
}