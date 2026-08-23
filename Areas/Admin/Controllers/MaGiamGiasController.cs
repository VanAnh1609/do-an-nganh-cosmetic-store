using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CosmeticStore.Data;
using CosmeticStore.Models;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MaGiamGiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaGiamGiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MaGiamGias
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaGiamGias.ToListAsync());
        }

        // GET: Admin/MaGiamGias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maGiamGia = await _context.MaGiamGias
                .FirstOrDefaultAsync(m => m.MaGiamGiaId == id);
            if (maGiamGia == null)
            {
                return NotFound();
            }

            return View(maGiamGia);
        }

        // GET: Admin/MaGiamGias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MaGiamGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
     [Bind("MaGiamGiaId,TenMa,MoTa,PhanTramGiam,GiamToiDa,DonHangToiThieu,NgayBatDau,NgayKetThuc,SoLuong,TrangThai")]
    MaGiamGia maGiamGia)
        {
            maGiamGia.TenMa = maGiamGia.TenMa.Trim().ToUpper();

            bool trungMa = await _context.MaGiamGias
                .AnyAsync(x => x.TenMa == maGiamGia.TenMa);

            if (trungMa)
            {
                ModelState.AddModelError(
                    "TenMa",
                    "Mã giảm giá này đã tồn tại"
                );
            }

            if (maGiamGia.NgayKetThuc < maGiamGia.NgayBatDau)
            {
                ModelState.AddModelError(
                    "NgayKetThuc",
                    "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu"
                );
            }

            if (ModelState.IsValid)
            {
                _context.Add(maGiamGia);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(maGiamGia);
        }

        // POST: Admin/MaGiamGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("MaGiamGiaId,TenMa,MoTa,PhanTramGiam,GiamToiDa,DonHangToiThieu,NgayBatDau,NgayKetThuc,SoLuong,TrangThai")]
    MaGiamGia maGiamGia)
        {
            if (id != maGiamGia.MaGiamGiaId)
            {
                return NotFound();
            }

            maGiamGia.TenMa = maGiamGia.TenMa.Trim().ToUpper();

            bool trungMa = await _context.MaGiamGias
                .AnyAsync(x =>
                    x.TenMa == maGiamGia.TenMa &&
                    x.MaGiamGiaId != maGiamGia.MaGiamGiaId);

            if (trungMa)
            {
                ModelState.AddModelError(
                    "TenMa",
                    "Mã giảm giá này đã tồn tại"
                );
            }

            if (maGiamGia.NgayKetThuc < maGiamGia.NgayBatDau)
            {
                ModelState.AddModelError(
                    "NgayKetThuc",
                    "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu"
                );
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maGiamGia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaGiamGiaExists(maGiamGia.MaGiamGiaId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(maGiamGia);
        }

        // GET: Admin/MaGiamGias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maGiamGia = await _context.MaGiamGias
                .FirstOrDefaultAsync(m => m.MaGiamGiaId == id);
            if (maGiamGia == null)
            {
                return NotFound();
            }

            return View(maGiamGia);
        }

        // POST: Admin/MaGiamGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maGiamGia = await _context.MaGiamGias.FindAsync(id);
            if (maGiamGia != null)
            {
                _context.MaGiamGias.Remove(maGiamGia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaGiamGiaExists(int id)
        {
            return _context.MaGiamGias.Any(e => e.MaGiamGiaId == id);
        }
    }
}
