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

        public async Task<IActionResult> Index()
        {
            // 5 sản phẩm mới 
            var sanPhamMoi = await _context.SanPhams
                .AsNoTracking()
                .Include(sp => sp.DanhMuc)
                .Where(sp => sp.TrangThai)
                .OrderByDescending(sp => sp.MaSanPham)
                .Take(5)
                .ToListAsync();

            // Thêm thương hiệu nổi bật
            var thuongHieuNoiBat = await _context.ThuongHieus
                .AsNoTracking()
                .Where(th => th.TrangThai)
                .OrderBy(th => th.TenThuongHieu)
                .Take(8)
                .ToListAsync();

            ViewBag.ThuongHieuNoiBat = thuongHieuNoiBat;

            return View(sanPhamMoi);
        }

        public IActionResult Privacy()
        {
            return View();
        }

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