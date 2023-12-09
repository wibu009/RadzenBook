using System.ComponentModel.DataAnnotations;

namespace RadzenBook.Client.Models
{
    public class CreateUnauthorizedUserInfoDTO
    {
        [Required(ErrorMessage = "Nhập tên của bạn")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Email không thể để trống"), EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống"), DataType(DataType.PhoneNumber, ErrorMessage = "Số điện thoại không đúng")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }
    }
}
