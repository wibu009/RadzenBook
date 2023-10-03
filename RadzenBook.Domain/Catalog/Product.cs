namespace RadzenBook.Domain.Catalog;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public string PictureUri { get; set; } = default!;
    public int CatalogTypeId { get; set; }
}