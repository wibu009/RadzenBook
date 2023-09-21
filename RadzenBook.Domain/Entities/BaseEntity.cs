using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Domain.Entities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    [MaxLength(100)]
    public string CreatedBy { get; set; } = "System";
    [MaxLength(100)]
    public string ModifiedBy { get; set; } = "System";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}