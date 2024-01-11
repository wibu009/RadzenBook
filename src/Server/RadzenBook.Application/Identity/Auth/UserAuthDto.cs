namespace RadzenBook.Application.Identity.Auth;

public class UserAuthDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}