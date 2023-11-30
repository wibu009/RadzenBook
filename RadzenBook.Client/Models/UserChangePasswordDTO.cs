using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Client.Models
{
    public class UserChangePasswordDTO
    {
        [Required(ErrorMessage = "Pasword is required"), StringLength(100, MinimumLength = 6, ErrorMessage = "Password have to shorter than 100 characters & minimum characters is 6")]
        public string NewPassword { get; set; } = string.Empty;
        [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
