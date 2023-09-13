namespace RadzenBook.Entity;

public class Photo : BaseEntity<string>
{
    public string Url { get; set; } = default!;
    public bool IsMain { get; set; } = false;
}