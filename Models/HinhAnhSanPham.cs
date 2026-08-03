using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class HinhAnhSanPham
    {
        [Key]
        public int MaHinhAnh { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Đường dẫn hình ảnh")]
        public string DuongDan { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Mô tả hình ảnh")]
        public string? MoTa { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public bool LaAnhDaiDien { get; set; } = false;

        [Display(Name = "Thứ tự hiển thị")]
        public int ThuTu { get; set; }

        public int MaSanPham { get; set; }

        public SanPham? SanPham { get; set; }
    }
}