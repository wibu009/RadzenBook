namespace RadzenBook.Entity;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public string CreatedBy { get; set; } = "System";
    public string ModifiedBy { get; set; } = "System";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}