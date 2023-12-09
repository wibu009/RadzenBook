using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Client.Models
{
    public class UserChangePasswordDTO
    {
        [Required(ErrorMessage = "Không được để trống mật khẩu"), StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải chứa ít nhất 6 ký tự & không dài quá 100 ký tự")]
        public string NewPassword { get; set; } = string.Empty;
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không khớp")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
