namespace RadzenBook.Domain.Catalog;

public class Author : BaseEntity<Guid>
{
    public string? FullName { get; set; }
    public string? Alias { get; set; }
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
}