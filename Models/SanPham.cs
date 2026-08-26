using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CosmeticStore.Models
{
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string TenSanPham { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá bán")]
        [DisplayFormat(
        DataFormatString = "{0:0}",
        ApplyFormatInEditMode = true)]
        public decimal GiaBan { get; set; }

        [Display(Name = "Số lượng tồn")]
        public int SoLuongTon { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? HinhAnh { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;

        // Khóa ngoại liên kết tới DanhMuc
        [ForeignKey(nameof(DanhMuc))]
        public int MaDanhMuc { get; set; }

        // Khóa ngoại liên kết tới ThuongHieu
        [ForeignKey(nameof(ThuongHieu))]
        public int MaThuongHieu { get; set; }

        // Navigation Property
        public DanhMuc? DanhMuc { get; set; }

        public ThuongHieu? ThuongHieu { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
            = new List<ChiTietDonHang>();
        public ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
            = new List<HinhAnhSanPham>();

        public ICollection<DanhGia> DanhGias { get; set; }
            = new List<DanhGia>();

        public ICollection<YeuThich> YeuThichs { get; set; }
            = new List<YeuThich>();
    }
}