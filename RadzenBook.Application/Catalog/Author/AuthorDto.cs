namespace RadzenBook.Application.Catalog.Author;

public class AuthorDto
{
    public Guid Id { get; set; } = default!;
    public string FullName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } = default;
    public DateTime DateOfDeath { get; set; } = default;
}