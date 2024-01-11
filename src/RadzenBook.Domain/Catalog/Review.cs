namespace RadzenBook.Domain.Catalog;

public class Review : BaseEntity<Guid>
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Rating { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
}