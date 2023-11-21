namespace RadzenBook.Domain.Catalog;

public class BookGenre : BaseEntity<Guid>
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = default!;
    public Guid GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
}