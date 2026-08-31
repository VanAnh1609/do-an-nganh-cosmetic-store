using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class ThuongHieu
    {
        [Key]
        public int MaThuongHieu { get; set; }

        [Required(ErrorMessage = "Tên thương hiệu không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên thương hiệu")]
        public string TenThuongHieu { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;
        [StringLength(500)]
        [Display(Name = "Logo thương hiệu")]
        public string? Logo { get; set; }
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}