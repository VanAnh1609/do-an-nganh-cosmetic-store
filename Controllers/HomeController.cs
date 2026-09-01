using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CosmeticStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        // =========================================================
        // TRANG CHỦ
        // =========================================================

        public async Task<IActionResult> Index()
        {
            // 5 sản phẩm mới
            var sanPhamMoi = await _context.SanPhams
                .AsNoTracking()
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.ThuongHieu)
                .Where(sp => sp.TrangThai)
                .OrderByDescending(sp => sp.MaSanPham)
                .Take(5)
                .ToListAsync();

            // 8 thương hiệu nổi bật
            var thuongHieuNoiBat = await _context.ThuongHieus
                .AsNoTracking()
                .Where(th => th.TrangThai)
                .OrderBy(th => th.TenThuongHieu)
                .Take(8)
                .ToListAsync();

            ViewBag.ThuongHieuNoiBat = thuongHieuNoiBat;

            return View(sanPhamMoi);
        }


        // =========================================================
        // LIÊN HỆ - GET
        // =========================================================

        [HttpGet]
        public IActionResult LienHe()
        {
            return View();
        }


        // =========================================================
        // LIÊN HỆ - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LienHe(LienHeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var lienHe = new LienHe
            {
                HoTen = model.HoTen,
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                NoiDung = model.NoiDung,
                NgayGui = DateTime.Now,
                DaXuLy = false
            };

            _context.LienHes.Add(lienHe);
            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Cảm ơn bạn đã liên hệ với Annie Cosmetics. Chúng tôi sẽ phản hồi sớm nhất.";

            return RedirectToAction(nameof(LienHe));
        }


        // =========================================================
        // PRIVACY
        // =========================================================

        public IActionResult Privacy()
        {
            return View();
        }


        // =========================================================
        // ERROR
        // =========================================================

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}