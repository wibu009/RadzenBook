namespace RadzenBook.Domain.Catalog;

public class Author : BaseEntity<Guid>
{
    public string FullName { get; set; } = default!;
    public string Alias { get; set; } = default!;
    public string Biography { get; set; } = default!;
    public string? ImageUrl { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; } = default!;
    public DateTime? DateOfDeath { get; set; } = default!;
    public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
}