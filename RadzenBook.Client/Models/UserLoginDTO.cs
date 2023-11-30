using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Client.Models
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "The Email field is not valid")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
