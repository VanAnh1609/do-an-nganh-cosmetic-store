using CosmeticStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KhachHang> KhachHangs { get; set; }

        public DbSet<DanhMuc> DanhMucs { get; set; }

        public DbSet<ThuongHieu> ThuongHieus { get; set; }

        public DbSet<SanPham> SanPhams { get; set; }

        public DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }

        public DbSet<DonHang> DonHangs { get; set; }

        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public DbSet<DanhGia> DanhGias { get; set; }

        public DbSet<MaGiamGia> MaGiamGias { get; set; }

        public DbSet<YeuThich> YeuThichs { get; set; }
    }
}