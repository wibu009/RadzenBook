namespace RadzenBook.Domain.Catalog;

public class Genre : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public virtual ICollection<BookGenre> Books { get; set; } = new HashSet<BookGenre>();
}