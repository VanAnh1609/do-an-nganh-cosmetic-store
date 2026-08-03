using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTietDonHang { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đơn giá")]
        public decimal DonGia { get; set; }

        // Khóa ngoại
        public int MaDonHang { get; set; }

        public int MaSanPham { get; set; }

        // Navigation Property
        public DonHang? DonHang { get; set; }

        public SanPham? SanPham { get; set; }
    }
}