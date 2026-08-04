using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    public class YeuThich
    {
        [Key]
        public int MaYeuThich { get; set; }

        // Khóa ngoại liên kết tới KhachHang
        [ForeignKey(nameof(KhachHang))]
        public int MaKhachHang { get; set; }

        // Khóa ngoại liên kết tới SanPham
        [ForeignKey(nameof(SanPham))]
        public int MaSanPham { get; set; }

        [Display(Name = "Ngày thêm")]
        public DateTime NgayThem { get; set; } = DateTime.Now;

        public KhachHang? KhachHang { get; set; }

        public SanPham? SanPham { get; set; }
    }
}