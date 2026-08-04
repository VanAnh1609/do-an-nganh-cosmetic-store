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

        // MaDonHang là khóa ngoại của navigation DonHang
        [ForeignKey(nameof(DonHang))]
        public int MaDonHang { get; set; }

        // MaSanPham là khóa ngoại của navigation SanPham
        [ForeignKey(nameof(SanPham))]
        public int MaSanPham { get; set; }

        public DonHang? DonHang { get; set; }

        public SanPham? SanPham { get; set; }
    }
}