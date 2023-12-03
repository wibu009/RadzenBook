namespace RadzenBook.Domain.Catalog;

public class Book : BaseEntity<Guid>
{
<<<<<<< HEAD
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
=======
>>>>>>> origin/kienct
    public string ISBN { get; set; } = default!;
    public string Language { get; set; } = default!;
    public int PageCount { get; set; } = default!;
    public decimal Weight { get; set; } = default!;
    public decimal Width { get; set; } = default!;
    public decimal Height { get; set; } = default!;
    public decimal Depth { get; set; } = default!;
    public int Republish { get; set; } = default!;
    public CoverType CoverType { get; set; } = default!;
    public DateTime PublishDate { get; set; } = default!;
    public Guid AuthorId { get; set; } = default!;
    public virtual Author Author { get; set; } = new();
    public Guid PublisherId { get; set; } = default!;
    public virtual Publisher Publisher { get; set; } = new();
    public Guid ProductId { get; set; } = default!;
    public virtual Product Product { get; set; } = new();
    public virtual ICollection<BookGenre> Genres { get; set; } = new HashSet<BookGenre>();
}