namespace RadzenBook.Application.Catalog.ProductImage;

public class ProductImageDto
{
    public string Id { get; set; } = default!;
    public string Url { get; set; } = default!;
    public bool IsMain { get; set; } = false;
}