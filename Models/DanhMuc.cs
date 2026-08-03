using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string TenDanhMuc { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}