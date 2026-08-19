using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticStore.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhMucsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhMucsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DanhMucs
        public async Task<IActionResult> Index(string? tuKhoa)
        {
            var query = _context.DanhMucs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(dm =>
            dm.TenDanhMuc.Contains(tuKhoa));

            }
            var danhMucs = await query
        .OrderBy(dm => dm.MaDanhMuc)
        .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;

            return View(danhMucs);
        }

        // GET: DanhMucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // GET: DanhMucs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DanhMucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDanhMuc,TenDanhMuc,MoTa,TrangThai")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // GET: DanhMucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        // POST: DanhMucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("MaDanhMuc,TenDanhMuc,MoTa")]
    DanhMuc danhMuc)
        {
            if (id != danhMuc.MaDanhMuc)
            {
                return NotFound();
            }

            // Kiểm tra tên danh mục có bị trùng không
            var tenDaTonTai = await _context.DanhMucs
                .AnyAsync(dm =>
                    dm.MaDanhMuc != id &&
                    dm.TenDanhMuc.ToLower() ==
                    danhMuc.TenDanhMuc.ToLower());

            if (tenDaTonTai)
            {
                ModelState.AddModelError(
                    "TenDanhMuc",
                    "Tên danh mục đã tồn tại."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(danhMuc);
            }

            // Lấy danh mục hiện tại từ database
            var danhMucHienTai = await _context.DanhMucs
                .FindAsync(id);

            if (danhMucHienTai == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật tên và mô tả
            danhMucHienTai.TenDanhMuc = danhMuc.TenDanhMuc;
            danhMucHienTai.MoTa = danhMuc.MoTa;

            // TrangThai KHÔNG thay đổi

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã cập nhật danh mục.";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var danhMuc = await _context.DanhMucs.FindAsync(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            danhMuc.TrangThai = !danhMuc.TrangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: DanhMucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // POST: DanhMucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMuc = await _context.DanhMucs
       .Include(dm => dm.SanPhams)
       .FirstOrDefaultAsync(dm => dm.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            if (danhMuc.SanPhams.Any())
            {
                TempData["ThongBaoLoi"] =
                    "Không thể xóa danh mục đang có sản phẩm. Bạn có thể ẩn danh mục thay thế.";

                return RedirectToAction(nameof(Index));
            }

            _context.DanhMucs.Remove(danhMuc);

            await _context.SaveChangesAsync();
            TempData["ThongBaoThanhCong"] =
        "Đã xóa danh mục.";
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucExists(int id)
        {
            return _context.DanhMucs.Any(e => e.MaDanhMuc == id);
        }
    }
}
