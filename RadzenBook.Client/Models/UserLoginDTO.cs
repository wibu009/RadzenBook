using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Client.Models
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Không được để trống Email"), EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Không được để trống mật khẩu")]
        public string Password { get; set; } = string.Empty;
    }
}
