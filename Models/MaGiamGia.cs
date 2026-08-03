using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    public class MaGiamGia
    {
        [Key]
        public int MaGiamGiaId { get; set; }

        [Required(ErrorMessage = "Mã giảm giá không được để trống")]
        [StringLength(50)]
        [Display(Name = "Mã giảm giá")]
        public string TenMa { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Range(0, 100)]
        [Display(Name = "Phần trăm giảm")]
        public int PhanTramGiam { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Số tiền giảm tối đa")]
        public decimal? GiamToiDa { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá trị đơn hàng tối thiểu")]
        public decimal? DonHangToiThieu { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime NgayKetThuc { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;

        public ICollection<DonHang> DonHangs { get; set; }
            = new List<DonHang>();
    }
}