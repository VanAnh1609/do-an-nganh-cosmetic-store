using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    public class ChiTietPhieuNhap
    {
        [Key]
        public int MaChiTietPhieuNhap { get; set; }

        [ForeignKey(nameof(PhieuNhap))]
        public int MaPhieuNhap { get; set; }

        [ForeignKey(nameof(SanPham))]
        public int MaSanPham { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá nhập phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá nhập")]
        public decimal GiaNhap { get; set; }

        [NotMapped]
        [Display(Name = "Thành tiền")]
        public decimal ThanhTien => SoLuong * GiaNhap;

        public PhieuNhap? PhieuNhap { get; set; }

        public SanPham? SanPham { get; set; }
    }
}