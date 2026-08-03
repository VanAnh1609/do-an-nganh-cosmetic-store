using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKhachHang { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Display(Name = "Mật khẩu")]
        public string MatKhauHash { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(500)]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [StringLength(30)]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } = "KhachHang";

        [Display(Name = "Ngày đăng ký")]
        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;

        public ICollection<DonHang> DonHangs { get; set; }
            = new List<DonHang>();

        public ICollection<DanhGia> DanhGias { get; set; }
            = new List<DanhGia>();

        public ICollection<YeuThich> YeuThichs { get; set; }
            = new List<YeuThich>();


    }
}