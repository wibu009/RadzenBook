namespace RadzenBook.Contract.DTO.Photo;

public class PhotoDto
{
    public string Id { get; set; } = default!;
    public string Url { get; set; } = default!;
    public bool IsMain { get; set; } = false;
}