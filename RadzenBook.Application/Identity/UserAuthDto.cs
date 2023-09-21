namespace RadzenBook.Application.Identity;

public class UserAuthDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}