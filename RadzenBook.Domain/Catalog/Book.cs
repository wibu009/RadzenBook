namespace RadzenBook.Domain.Catalog;

public class Book : BaseEntity<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public string? Language { get; set; }
    public string? Translator { get; set; }
    public int PageCount { get; set; }
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public int Republish { get; set; }
    public CoverType CoverType { get; set; }
    public DateTime? PublishDate { get; set; }
    public Guid? AuthorId { get; set; }
    public virtual Author Author { get; set; } = new();
    public Guid? PublisherId { get; set; } = default!;
    public virtual Publisher Publisher { get; set; } = new();
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = new();
    public virtual ICollection<BookGenre> Genres { get; set; } = new HashSet<BookGenre>();
}