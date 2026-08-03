using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        public int MaKhachHang { get; set; }

        public int MaSanPham { get; set; }

        [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
        [Display(Name = "Số sao")]
        public int SoSao { get; set; }

        [StringLength(1000)]
        [Display(Name = "Bình luận")]
        public string? BinhLuan { get; set; }

        [Display(Name = "Ngày đánh giá")]
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        [Display(Name = "Đã duyệt")]
        public bool DaDuyet { get; set; } = false;

        public KhachHang? KhachHang { get; set; }

        public SanPham? SanPham { get; set; }
    }
}