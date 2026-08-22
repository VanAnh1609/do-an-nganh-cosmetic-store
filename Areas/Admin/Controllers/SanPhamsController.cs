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
    public class SanPhamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanPhamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index(
             string? tuKhoa,
             int? maDanhMuc,
             int? maThuongHieu,
             bool? trangThai)
        {
            var query = _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.ThuongHieu)
                .AsQueryable();

            // Tìm theo tên sản phẩm
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(sp =>
                    sp.TenSanPham.Contains(tuKhoa));
            }

            // Lọc danh mục
            if (maDanhMuc.HasValue)
            {
                query = query.Where(sp =>
                    sp.MaDanhMuc == maDanhMuc.Value);
            }

            // Lọc thương hiệu
            if (maThuongHieu.HasValue)
            {
                query = query.Where(sp =>
                    sp.MaThuongHieu == maThuongHieu.Value);
            }

            // Lọc trạng thái
            if (trangThai.HasValue)
            {
                query = query.Where(sp =>
                    sp.TrangThai == trangThai.Value);
            }

            var sanPhams = await query
                .OrderBy(sp => sp.MaSanPham)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.MaThuongHieu = maThuongHieu;
            ViewBag.TrangThai = trangThai;

            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs
                    .OrderBy(dm => dm.TenDanhMuc)
                    .ToListAsync(),
                "MaDanhMuc",
                "TenDanhMuc",
                maDanhMuc);

            ViewBag.ThuongHieus = new SelectList(
                await _context.ThuongHieus
                    .OrderBy(th => th.TenThuongHieu)
                    .ToListAsync(),
                "MaThuongHieu",
                "TenThuongHieu",
                maThuongHieu);

            return View(sanPhams);
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.ThuongHieu)
                .FirstOrDefaultAsync(sp =>
                sp.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs
                    .Where(dm => dm.TrangThai)
                    .OrderBy(dm => dm.TenDanhMuc)
                    .ToListAsync(),
                "MaDanhMuc",
                "TenDanhMuc");

            ViewBag.ThuongHieus = new SelectList(
                await _context.ThuongHieus
                    .Where(th => th.TrangThai)
                    .OrderBy(th => th.TenThuongHieu)
                    .ToListAsync(),
                "MaThuongHieu",
                "TenThuongHieu");

            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("TenSanPham,MoTa,GiaBan,SoLuongTon,HinhAnh,TrangThai,MaDanhMuc,MaThuongHieu")]
    SanPham sanPham)
        {
            var tenDaTonTai = await _context.SanPhams
                .AnyAsync(sp =>
                    sp.TenSanPham.ToLower() ==
                    sanPham.TenSanPham.ToLower());

            if (tenDaTonTai)
            {
                ModelState.AddModelError(
                    "TenSanPham",
                    "Tên sản phẩm đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                sanPham.NgayTao = DateTime.Now;

                _context.SanPhams.Add(sanPham);

                await _context.SaveChangesAsync();

                TempData["ThongBaoThanhCong"] =
                    "Đã thêm sản phẩm mới.";

                return RedirectToAction(nameof(Index));
            }

            // Nếu form lỗi thì phải load lại dropdown
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs
                    .Where(dm => dm.TrangThai)
                    .OrderBy(dm => dm.TenDanhMuc)
                    .ToListAsync(),
                "MaDanhMuc",
                "TenDanhMuc",
                sanPham.MaDanhMuc);

            ViewBag.ThuongHieus = new SelectList(
                await _context.ThuongHieus
                    .Where(th => th.TrangThai)
                    .OrderBy(th => th.TenThuongHieu)
                    .ToListAsync(),
                "MaThuongHieu",
                "TenThuongHieu",
                sanPham.MaThuongHieu);

            return View(sanPham);
        }
        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs
                    .Where(dm => dm.TrangThai)
                    .OrderBy(dm => dm.TenDanhMuc)
                    .ToListAsync(),
                "MaDanhMuc",
                "TenDanhMuc",
                sanPham.MaDanhMuc);

            ViewBag.ThuongHieus = new SelectList(
                await _context.ThuongHieus
                    .Where(th => th.TrangThai)
                    .OrderBy(th => th.TenThuongHieu)
                    .ToListAsync(),
                "MaThuongHieu",
                "TenThuongHieu",
                sanPham.MaThuongHieu);

            return View(sanPham);
        }
        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("MaSanPham,TenSanPham,MoTa,GiaBan,SoLuongTon,HinhAnh,MaDanhMuc,MaThuongHieu")]
    SanPham sanPham)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }

            var tenDaTonTai = await _context.SanPhams
                .AnyAsync(sp =>
                    sp.MaSanPham != id &&
                    sp.TenSanPham.ToLower() ==
                    sanPham.TenSanPham.ToLower());

            if (tenDaTonTai)
            {
                ModelState.AddModelError(
                    "TenSanPham",
                    "Tên sản phẩm đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DanhMucs = new SelectList(
                    await _context.DanhMucs
                        .Where(dm => dm.TrangThai)
                        .OrderBy(dm => dm.TenDanhMuc)
                        .ToListAsync(),
                    "MaDanhMuc",
                    "TenDanhMuc",
                    sanPham.MaDanhMuc);

                ViewBag.ThuongHieus = new SelectList(
                    await _context.ThuongHieus
                        .Where(th => th.TrangThai)
                        .OrderBy(th => th.TenThuongHieu)
                        .ToListAsync(),
                    "MaThuongHieu",
                    "TenThuongHieu",
                    sanPham.MaThuongHieu);

                return View(sanPham);
            }

            var sanPhamHienTai =
                await _context.SanPhams.FindAsync(id);

            if (sanPhamHienTai == null)
            {
                return NotFound();
            }

            sanPhamHienTai.TenSanPham = sanPham.TenSanPham;
            sanPhamHienTai.MoTa = sanPham.MoTa;
            sanPhamHienTai.GiaBan = sanPham.GiaBan;
            sanPhamHienTai.SoLuongTon = sanPham.SoLuongTon;
            sanPhamHienTai.HinhAnh = sanPham.HinhAnh;
            sanPhamHienTai.MaDanhMuc = sanPham.MaDanhMuc;
            sanPhamHienTai.MaThuongHieu = sanPham.MaThuongHieu;

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã cập nhật sản phẩm.";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            sanPham.TrangThai = !sanPham.TrangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.ChiTietDonHangs)
                .FirstOrDefaultAsync(sp => sp.MaSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Sản phẩm đã từng xuất hiện trong đơn hàng
            if (sanPham.ChiTietDonHangs.Any())
            {
                TempData["ThongBaoLoi"] =
                    "Không thể xóa sản phẩm đã có trong đơn hàng. Hãy ẩn sản phẩm thay thế.";

                return RedirectToAction(nameof(Index));
            }

            _context.SanPhams.Remove(sanPham);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã xóa sản phẩm thành công.";

            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSanPham == id);
        }
    }
}
