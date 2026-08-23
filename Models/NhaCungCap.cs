using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class NhaCungCap
    {
        [Key]
        public int MaNhaCungCap { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [StringLength(200)]
        [Display(Name = "Tên nhà cung cấp")]
        public string TenNhaCungCap { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(300)]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;

        public ICollection<PhieuNhap> PhieuNhaps { get; set; }
            = new List<PhieuNhap>();
    }
}