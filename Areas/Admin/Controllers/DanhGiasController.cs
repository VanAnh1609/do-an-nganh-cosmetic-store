using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhGiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhGiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách đánh giá
        public async Task<IActionResult> Index()
        {
            var danhGias = await _context.DanhGias
                .Include(x => x.KhachHang)
                .Include(x => x.SanPham)
                .OrderByDescending(x => x.NgayDanhGia)
                .ToListAsync();

            return View(danhGias);
        }

        // Xem chi tiết
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var danhGia = await _context.DanhGias
                .Include(x => x.KhachHang)
                .Include(x => x.SanPham)
                .FirstOrDefaultAsync(x => x.MaDanhGia == id);

            if (danhGia == null)
                return NotFound();

            return View(danhGia);
        }

        // Duyệt đánh giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duyet(int id)
        {
            var danhGia = await _context.DanhGias.FindAsync(id);

            if (danhGia == null)
                return NotFound();

            danhGia.DaDuyet = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Bỏ duyệt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoDuyet(int id)
        {
            var danhGia = await _context.DanhGias.FindAsync(id);

            if (danhGia == null)
                return NotFound();

            danhGia.DaDuyet = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Trang xác nhận xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var danhGia = await _context.DanhGias
                .Include(x => x.KhachHang)
                .Include(x => x.SanPham)
                .FirstOrDefaultAsync(x => x.MaDanhGia == id);

            if (danhGia == null)
                return NotFound();

            return View(danhGia);
        }

        // Xác nhận xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhGia = await _context.DanhGias.FindAsync(id);

            if (danhGia != null)
            {
                _context.DanhGias.Remove(danhGia);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}