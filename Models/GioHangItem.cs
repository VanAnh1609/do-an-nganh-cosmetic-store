namespace CosmeticStore.Models
{
    public class GioHangItem
    {
        public int MaSanPham { get; set; }

        public string TenSanPham { get; set; } = string.Empty;

        public string? HinhAnh { get; set; }

        public decimal GiaBan { get; set; }

        public int SoLuong { get; set; }

        public decimal ThanhTien
        {
            get
            {
                return GiaBan * SoLuong;
            }
        }
    }
}