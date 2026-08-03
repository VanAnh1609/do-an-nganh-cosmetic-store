using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class YeuThich
    {
        [Key]
        public int MaYeuThich { get; set; }

        public int MaKhachHang { get; set; }

        public int MaSanPham { get; set; }

        [Display(Name = "Ngày thêm")]
        public DateTime NgayThem { get; set; } = DateTime.Now;

        public KhachHang? KhachHang { get; set; }

        public SanPham? SanPham { get; set; }
    }
}