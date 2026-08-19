using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CosmeticStore.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ThuongHieusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThuongHieusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThuongHieus
        public async Task<IActionResult> Index(string? tuKhoa)
        {
            var query = _context.ThuongHieus.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(th =>
                    th.TenThuongHieu.Contains(tuKhoa));
            }

            var thuongHieus = await query
                .OrderBy(th => th.MaThuongHieu)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;

            return View(thuongHieus);
        }

        // GET: ThuongHieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.MaThuongHieu == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // GET: ThuongHieus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ThuongHieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
     [Bind("MaThuongHieu,TenThuongHieu,MoTa,TrangThai")]
    ThuongHieu thuongHieu)
        {
            var tenDaTonTai = await _context.ThuongHieus
                .AnyAsync(th =>
                    th.TenThuongHieu.ToLower() ==
                    thuongHieu.TenThuongHieu.ToLower());

            if (tenDaTonTai)
            {
                ModelState.AddModelError(
                    "TenThuongHieu",
                    "Tên thương hiệu đã tồn tại."
                );
            }

            if (ModelState.IsValid)
            {
                _context.ThuongHieus.Add(thuongHieu);

                await _context.SaveChangesAsync();

                TempData["ThongBaoThanhCong"] =
                    "Đã thêm thương hiệu mới.";

                return RedirectToAction(nameof(Index));
            }

            return View(thuongHieu);
        }

        // GET: ThuongHieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieu == null)
            {
                return NotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThuongHieu,TenThuongHieu,MoTa,TrangThai")] ThuongHieu thuongHieu)
        {
            if (id != thuongHieu.MaThuongHieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuongHieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuongHieuExists(thuongHieu.MaThuongHieu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(thuongHieu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var thuongHieu = await _context.ThuongHieus.FindAsync(id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            thuongHieu.TrangThai = !thuongHieu.TrangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ThuongHieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.MaThuongHieu == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // POST: ThuongHieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuongHieu = await _context.ThuongHieus
                .Include(th => th.SanPhams)
                .FirstOrDefaultAsync(th => th.MaThuongHieu == id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            if (thuongHieu.SanPhams.Any())
            {
                TempData["ThongBaoLoi"] =
                    "Không thể xóa thương hiệu đang có sản phẩm. Hãy ẩn thương hiệu thay thế.";

                return RedirectToAction(nameof(Index));
            }

            _context.ThuongHieus.Remove(thuongHieu);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã xóa thương hiệu thành công.";

            return RedirectToAction(nameof(Index));
        }

        private bool ThuongHieuExists(int id)
        {
            return _context.ThuongHieus.Any(e => e.MaThuongHieu == id);
        }
    }
}
