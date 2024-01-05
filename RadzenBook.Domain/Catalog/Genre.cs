namespace RadzenBook.Domain.Catalog;

public class Genre : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<BookGenre> Books { get; set; } = new HashSet<BookGenre>();
}