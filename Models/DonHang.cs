using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        public int MaKhachHang { get; set; }

        public int? MaGiamGiaId { get; set; }

        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên người nhận")]
        public string TenNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        [StringLength(500)]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiaoHang { get; set; } = string.Empty;

        [Display(Name = "Ngày đặt")]
        public DateTime NgayDat { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tiền giảm")]
        public decimal TienGiam { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Phí vận chuyển")]
        public decimal PhiVanChuyen { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Trạng thái đơn hàng")]
        public string TrangThai { get; set; } = "ChoXacNhan";

        [Required]
        [StringLength(50)]
        [Display(Name = "Phương thức thanh toán")]
        public string PhuongThucThanhToan { get; set; } = "COD";

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        public KhachHang? KhachHang { get; set; }

        public MaGiamGia? MaGiamGia { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
            = new List<ChiTietDonHang>();
    }
}