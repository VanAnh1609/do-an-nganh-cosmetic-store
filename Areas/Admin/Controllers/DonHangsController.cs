using CosmeticStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangsController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách đơn hàng dành cho Admin
        public async Task<IActionResult> Index(
    string? tuKhoa,
    string? trangThai)
        {
            var query = _context.DonHangs
                .AsNoTracking()
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                .AsQueryable();

            // Tìm kiếm theo mã đơn, tên người nhận hoặc email khách hàng
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                if (int.TryParse(tuKhoa.Replace("#", ""), out int maDon))
                {
                    query = query.Where(dh =>
                        dh.MaDonHang == maDon ||
                        dh.TenNguoiNhan.Contains(tuKhoa) ||
                        (dh.KhachHang != null &&
                         dh.KhachHang.Email.Contains(tuKhoa)));
                }
                else
                {
                    query = query.Where(dh =>
                        dh.TenNguoiNhan.Contains(tuKhoa) ||
                        (dh.KhachHang != null &&
                         dh.KhachHang.Email.Contains(tuKhoa)));
                }
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(dh =>
                    dh.TrangThai == trangThai);
            }

            var donHangs = await query
                .OrderByDescending(dh => dh.NgayDat)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TrangThai = trangThai;

            return View(donHangs);
        }

        // Chi tiết đơn hàng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .AsNoTracking()
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.MaGiamGia)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(dh =>
                    dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // Admin cập nhật trạng thái đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThai(
     int id,
     string trangThai)
        {
            string[] trangThaiHopLe =
            {
        "ChoXacNhan",
        "DaXacNhan",
        "DangGiao",
        "DaGiao",
        "DaHuy",
        "YeuCauHoanHang",
        "DaHoanHang"
    };

            if (!trangThaiHopLe.Contains(trangThai))
            {
                TempData["ThongBaoLoi"] =
                    "Trạng thái đơn hàng không hợp lệ.";

                return RedirectToAction(
                    nameof(Details),
                    new { id });
            }

            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(dh =>
                    dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            var trangThaiCu = donHang.TrangThai;

            bool chuyenTrangThaiHopLe =
                (trangThaiCu == "ChoXacNhan" &&
                    (trangThai == "DaXacNhan" ||
                     trangThai == "DaHuy"))
                ||
                (trangThaiCu == "DaXacNhan" &&
                    trangThai == "DangGiao")
                ||
                (trangThaiCu == "DangGiao" &&
                    trangThai == "DaGiao");

            if (!chuyenTrangThaiHopLe)
            {
                TempData["ThongBaoLoi"] =
                    "Không thể chuyển trạng thái đơn hàng theo cách này.";

                return RedirectToAction(
                    nameof(Details),
                    new { id });
            }

            // Nếu Admin hủy đơn khi đang chờ xác nhận,
            // cộng lại tồn kho vì lúc khách đặt hàng
            // hệ thống đã trừ tồn rồi.
            if (trangThai == "DaHuy")
            {
                foreach (var chiTiet in donHang.ChiTietDonHangs)
                {
                    if (chiTiet.SanPham != null)
                    {
                        chiTiet.SanPham.SoLuongTon += chiTiet.SoLuong;
                    }
                }
            }

            donHang.TrangThai = trangThai;

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã cập nhật trạng thái đơn hàng.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DuyetHoanHang(int id)
        {
            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(dh =>
                    dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            if (donHang.TrangThai != "YeuCauHoanHang")
            {
                TempData["ThongBaoLoi"] =
                    "Đơn hàng này không có yêu cầu hoàn hàng.";

                return RedirectToAction(
                    nameof(Details),
                    new { id });
            }

            // Cộng số lượng sản phẩm trở lại kho
            foreach (var chiTiet in donHang.ChiTietDonHangs)
            {
                if (chiTiet.SanPham != null)
                {
                    chiTiet.SanPham.SoLuongTon += chiTiet.SoLuong;
                }
            }

            donHang.TrangThai = "DaHoanHang";

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã duyệt hoàn hàng và cập nhật lại tồn kho.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(dh =>
                dh.MaDonHang == id);
        }
    }
}