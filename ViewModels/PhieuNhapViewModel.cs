using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.ViewModels
{
    public class PhieuNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int MaNhaCungCap { get; set; }

        [Display(Name = "Ngày nhập")]
        public DateTime NgayNhap { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        public List<ChiTietPhieuNhapViewModel> ChiTietPhieuNhaps { get; set; }
            = new List<ChiTietPhieuNhapViewModel>();
    }

    public class ChiTietPhieuNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int MaSanPham { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Số lượng nhập phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Range(0.01, 999999999999.0,
    ErrorMessage = "Giá nhập phải lớn hơn 0")]
        [Display(Name = "Giá nhập")]
        public decimal GiaNhap { get; set; }
    }
}