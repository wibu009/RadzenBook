using RadzenBook.Domain.Common.Contracts;

namespace RadzenBook.Domain.Catalog;

public class Photo : BaseEntity<string>
{
    public string Url { get; set; } = default!;
    public bool IsMain { get; set; } = false;
}