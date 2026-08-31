using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ThuongHieusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ThuongHieusController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        // =========================================================
        // DANH SÁCH THƯƠNG HIỆU
        // =========================================================

        public async Task<IActionResult> Index(string? tuKhoa)
        {
            var query = _context.ThuongHieus
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                query = query.Where(th =>
                    th.TenThuongHieu.Contains(tuKhoa));
            }

            var thuongHieus = await query
                .OrderBy(th => th.MaThuongHieu)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;

            return View(thuongHieus);
        }


        // =========================================================
        // CHI TIẾT
        // =========================================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .AsNoTracking()
                .FirstOrDefaultAsync(th =>
                    th.MaThuongHieu == id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }


        // =========================================================
        // CREATE GET
        // =========================================================

        public IActionResult Create()
        {
            return View();
        }


        // =========================================================
        // CREATE POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MaThuongHieu,TenThuongHieu,MoTa,TrangThai")]
            ThuongHieu thuongHieu,
            IFormFile? logoFile)
        {
            if (!string.IsNullOrWhiteSpace(
                thuongHieu.TenThuongHieu))
            {
                thuongHieu.TenThuongHieu =
                    thuongHieu.TenThuongHieu.Trim();

                var tenDaTonTai =
                    await _context.ThuongHieus
                        .AnyAsync(th =>
                            th.TenThuongHieu.ToLower() ==
                            thuongHieu.TenThuongHieu.ToLower());

                if (tenDaTonTai)
                {
                    ModelState.AddModelError(
                        "TenThuongHieu",
                        "Tên thương hiệu đã tồn tại.");
                }
            }

            KiemTraLogo(logoFile);

            if (!ModelState.IsValid)
            {
                return View(thuongHieu);
            }

            if (logoFile != null &&
                logoFile.Length > 0)
            {
                thuongHieu.Logo =
                    await LuuLogoThuongHieu(logoFile);
            }

            thuongHieu.MoTa =
                string.IsNullOrWhiteSpace(
                    thuongHieu.MoTa)
                    ? null
                    : thuongHieu.MoTa.Trim();

            _context.ThuongHieus.Add(thuongHieu);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã thêm thương hiệu mới.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // EDIT GET
        // =========================================================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu =
                await _context.ThuongHieus
                    .FindAsync(id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }


        // =========================================================
        // EDIT POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string TenThuongHieu,
            string? MoTa,
            bool TrangThai,
            IFormFile? logoFile)
        {
            var thuongHieu =
                await _context.ThuongHieus
                    .FirstOrDefaultAsync(th =>
                        th.MaThuongHieu == id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(
                TenThuongHieu))
            {
                ModelState.AddModelError(
                    "TenThuongHieu",
                    "Tên thương hiệu không được để trống.");
            }
            else
            {
                TenThuongHieu =
                    TenThuongHieu.Trim();

                var tenDaTonTai =
                    await _context.ThuongHieus
                        .AnyAsync(th =>
                            th.MaThuongHieu != id &&
                            th.TenThuongHieu.ToLower() ==
                            TenThuongHieu.ToLower());

                if (tenDaTonTai)
                {
                    ModelState.AddModelError(
                        "TenThuongHieu",
                        "Tên thương hiệu đã tồn tại.");
                }
            }

            KiemTraLogo(logoFile);

            if (!ModelState.IsValid)
            {
                thuongHieu.TenThuongHieu =
                    TenThuongHieu;

                thuongHieu.MoTa = MoTa;
                thuongHieu.TrangThai = TrangThai;

                return View(thuongHieu);
            }

            thuongHieu.TenThuongHieu =
                TenThuongHieu;

            thuongHieu.MoTa =
                string.IsNullOrWhiteSpace(MoTa)
                    ? null
                    : MoTa.Trim();

            thuongHieu.TrangThai =
                TrangThai;


            // Nếu chọn logo mới
            if (logoFile != null &&
                logoFile.Length > 0)
            {
                XoaLogoCu(
                    thuongHieu.Logo);

                thuongHieu.Logo =
                    await LuuLogoThuongHieu(
                        logoFile);
            }

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã cập nhật thương hiệu.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // ĐỔI TRẠNG THÁI
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(
            int id)
        {
            var thuongHieu =
                await _context.ThuongHieus
                    .FindAsync(id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            thuongHieu.TrangThai =
                !thuongHieu.TrangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // DELETE GET
        // =========================================================

        public async Task<IActionResult> Delete(
            int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu =
                await _context.ThuongHieus
                    .AsNoTracking()
                    .FirstOrDefaultAsync(th =>
                        th.MaThuongHieu == id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }


        // =========================================================
        // DELETE POST
        // =========================================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var thuongHieu =
                await _context.ThuongHieus
                    .Include(th => th.SanPhams)
                    .FirstOrDefaultAsync(th =>
                        th.MaThuongHieu == id);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            if (thuongHieu.SanPhams.Any())
            {
                TempData["ThongBaoLoi"] =
                    "Không thể xóa thương hiệu đang có sản phẩm. Hãy ẩn thương hiệu thay thế.";

                return RedirectToAction(
                    nameof(Index));
            }


            // Xóa logo vật lý nếu có
            XoaLogoCu(
                thuongHieu.Logo);


            _context.ThuongHieus.Remove(
                thuongHieu);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã xóa thương hiệu thành công.";

            return RedirectToAction(
                nameof(Index));
        }


        // =========================================================
        // KIỂM TRA LOGO
        // =========================================================

        private void KiemTraLogo(
            IFormFile? logoFile)
        {
            if (logoFile == null ||
                logoFile.Length <= 0)
            {
                return;
            }

            var extension =
                Path.GetExtension(
                    logoFile.FileName)
                    .ToLowerInvariant();

            var allowedExtensions =
                new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp",
                    ".svg"
                };

            if (!allowedExtensions.Contains(
                extension))
            {
                ModelState.AddModelError(
                    "",
                    "Logo chỉ được sử dụng JPG, PNG, WEBP hoặc SVG.");
            }

            if (logoFile.Length >
                5 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    "",
                    "Dung lượng logo không được vượt quá 5MB.");
            }
        }


        // =========================================================
        // LƯU LOGO
        // =========================================================

        private async Task<string>
            LuuLogoThuongHieu(
                IFormFile logoFile)
        {
            var folderPath =
                Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "brands");

            if (!Directory.Exists(
                folderPath))
            {
                Directory.CreateDirectory(
                    folderPath);
            }

            var extension =
                Path.GetExtension(
                    logoFile.FileName)
                    .ToLowerInvariant();

            var fileName =
                $"{Guid.NewGuid()}{extension}";

            var filePath =
                Path.Combine(
                    folderPath,
                    fileName);

            await using var stream =
                new FileStream(
                    filePath,
                    FileMode.Create);

            await logoFile.CopyToAsync(
                stream);

            return
                $"/images/brands/{fileName}";
        }


        // =========================================================
        // XÓA LOGO CŨ
        // =========================================================

        private void XoaLogoCu(
            string? logoPath)
        {
            if (string.IsNullOrWhiteSpace(
                logoPath))
            {
                return;
            }

            var relativePath =
                logoPath
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar);

            var fullPath =
                Path.Combine(
                    _environment.WebRootPath,
                    relativePath);

            if (System.IO.File.Exists(
                fullPath))
            {
                System.IO.File.Delete(
                    fullPath);
            }
        }


        // =========================================================
        // EXISTS
        // =========================================================

        private bool ThuongHieuExists(
            int id)
        {
            return _context.ThuongHieus
                .Any(th =>
                    th.MaThuongHieu == id);
        }
    }
}