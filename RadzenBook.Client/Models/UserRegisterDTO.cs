using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Client.Models
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Không được để trống email"), EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mật khẩu không được để trống"), StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải chứa ít nhất 6 ký tự & không dài quá 100 ký tự")]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
