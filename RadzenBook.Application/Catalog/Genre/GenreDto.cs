namespace RadzenBook.Application.Catalog.Genre;

public class GenreDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int TotalBooks { get; set; }
}