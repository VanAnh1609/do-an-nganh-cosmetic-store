using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.ViewModels
{
    public class LienHeViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung liên hệ.")]
        [StringLength(
            1000,
            ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; } = string.Empty;
    }
}