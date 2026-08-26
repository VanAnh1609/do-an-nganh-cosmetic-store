using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CuaHangController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Danh sách sản phẩm phía khách hàng
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _context.SanPhams
                .Where(sp => sp.TrangThai)
                .ToListAsync();

            return View(sanPhams);
        }

        // Chi tiết sản phẩm
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhSanPhams)
                .Include(sp => sp.DanhGias)
                    .ThenInclude(dg => dg.KhachHang)
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == id &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DanhGiaSanPham(int id)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == id &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Kiểm tra khách đã mua sản phẩm và đơn đã giao
            bool daMuaSanPham = await _context.DonHangs
                .AnyAsync(dh =>
                    dh.MaKhachHang == khachHang.MaKhachHang &&
                    dh.TrangThai == "DaGiao" &&
                    dh.ChiTietDonHangs.Any(ct =>
                        ct.MaSanPham == id));

            if (!daMuaSanPham)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn chỉ có thể đánh giá sản phẩm đã mua và đã nhận hàng.";

                return RedirectToAction(
                    "DonHangCuaToi",
                    "GioHang");
            }

            // Nếu đã đánh giá rồi thì không cho đánh giá tiếp
            bool daDanhGia = await _context.DanhGias
                .AnyAsync(dg =>
                    dg.MaKhachHang == khachHang.MaKhachHang &&
                    dg.MaSanPham == id);

            if (daDanhGia)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn đã đánh giá sản phẩm này rồi.";

                return RedirectToAction(
                    "DonHangCuaToi",
                    "GioHang");
            }

            return View(sanPham);
        }

        // Khách hàng gửi đánh giá sản phẩm
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuiDanhGia(
            int maSanPham,
            int soSao,
            string? binhLuan)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            // Kiểm tra số sao
            if (soSao < 1 || soSao > 5)
            {
                TempData["ThongBaoLoi"] =
                    "Số sao phải từ 1 đến 5.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = maSanPham });
            }

            // Kiểm tra sản phẩm có tồn tại không
            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == maSanPham &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Chỉ khách đã mua và đơn đã giao mới được đánh giá
            bool daMuaSanPham = await _context.DonHangs
                .AnyAsync(dh =>
                    dh.MaKhachHang == khachHang.MaKhachHang &&
                    dh.TrangThai == "DaGiao" &&
                    dh.ChiTietDonHangs.Any(ct =>
                        ct.MaSanPham == maSanPham));

            if (!daMuaSanPham)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn chỉ có thể đánh giá sản phẩm đã mua và đã nhận hàng.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = maSanPham });
            }

            // Không cho cùng một khách đánh giá cùng sản phẩm nhiều lần
            bool daDanhGia = await _context.DanhGias
                .AnyAsync(dg =>
                    dg.MaKhachHang == khachHang.MaKhachHang &&
                    dg.MaSanPham == maSanPham);

            if (daDanhGia)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn đã đánh giá sản phẩm này rồi.";

                return RedirectToAction(
                 nameof(DanhGiaSanPham),
                 new { id = maSanPham });
            }

            var danhGia = new DanhGia
            {
                MaKhachHang = khachHang.MaKhachHang,
                MaSanPham = maSanPham,
                SoSao = soSao,
                BinhLuan = string.IsNullOrWhiteSpace(binhLuan)
                    ? null
                    : binhLuan.Trim(),
                NgayDanhGia = DateTime.Now,
                DaDuyet = false
            };

            _context.DanhGias.Add(danhGia);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã gửi đánh giá. Đánh giá sẽ hiển thị sau khi được Admin duyệt.";

            return RedirectToAction(
            nameof(DanhGiaSanPham),
            new { id = maSanPham });
        }
    }
}