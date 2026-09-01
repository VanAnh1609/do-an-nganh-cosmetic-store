using CosmeticStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LienHesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LienHesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // DANH SÁCH LIÊN HỆ
        // =====================================================

        public async Task<IActionResult> Index()
        {
            var lienHes = await _context.LienHes
                .AsNoTracking()
                .OrderBy(lh => lh.DaXuLy)
                .ThenByDescending(lh => lh.NgayGui)
                .ToListAsync();

            return View(lienHes);
        }


        // =====================================================
        // CHI TIẾT LIÊN HỆ
        // =====================================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.LienHes
                .AsNoTracking()
                .FirstOrDefaultAsync(lh => lh.LienHeId == id);

            if (lienHe == null)
            {
                return NotFound();
            }

            return View(lienHe);
        }


        // =====================================================
        // ĐÁNH DẤU ĐÃ XỬ LÝ
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DanhDauDaXuLy(int id)
        {
            var lienHe = await _context.LienHes.FindAsync(id);

            if (lienHe == null)
            {
                return NotFound();
            }

            lienHe.DaXuLy = true;

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã đánh dấu liên hệ là đã xử lý.";

            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // XÓA LIÊN HỆ
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var lienHe = await _context.LienHes.FindAsync(id);

            if (lienHe == null)
            {
                return NotFound();
            }

            _context.LienHes.Remove(lienHe);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã xóa liên hệ.";

            return RedirectToAction(nameof(Index));
        }
    }
}