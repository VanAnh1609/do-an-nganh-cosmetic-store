using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CosmeticStore.Data;
using CosmeticStore.Models;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KhachHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhachHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // DANH SÁCH KHÁCH HÀNG
        // GET: Admin/KhachHangs
        // =====================================================
        public async Task<IActionResult> Index(string? tuKhoa, bool? trangThai)
        {
            var query = _context.KhachHangs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(kh =>
                    kh.HoTen.Contains(tuKhoa) ||
                    kh.Email.Contains(tuKhoa) ||
                    (kh.SoDienThoai != null && kh.SoDienThoai.Contains(tuKhoa))
                );
            }

            if (trangThai.HasValue)
            {
                query = query.Where(kh => kh.TrangThai == trangThai.Value);
            }

            var khachHangs = await query
                .OrderByDescending(kh => kh.NgayDangKy)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TrangThai = trangThai;

            return View(khachHangs);
        }

        // =====================================================
        // CHI TIẾT KHÁCH HÀNG + LỊCH SỬ ĐƠN HÀNG
        // GET: Admin/KhachHangs/Details/5
        // =====================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(kh => kh.DonHangs)
                .FirstOrDefaultAsync(kh => kh.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // =====================================================
        // CHỈNH SỬA KHÁCH HÀNG
        // GET: Admin/KhachHangs/Edit/5
        // =====================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // =====================================================
        // LƯU CHỈNH SỬA KHÁCH HÀNG
        // POST: Admin/KhachHangs/Edit/5
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string HoTen,
            string Email,
            string? SoDienThoai,
            string? DiaChi)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                ModelState.AddModelError(
                    "HoTen",
                    "Họ tên không được để trống"
                );
            }

            // Kiểm tra email
            if (string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError(
                    "Email",
                    "Email không được để trống"
                );
            }

            // Nếu dữ liệu không hợp lệ
            if (!ModelState.IsValid)
            {
                khachHang.HoTen = HoTen;
                khachHang.Email = Email;
                khachHang.SoDienThoai = SoDienThoai;
                khachHang.DiaChi = DiaChi;

                return View(khachHang);
            }

            // Chỉ cập nhật những thông tin Admin được phép sửa
            khachHang.HoTen = HoTen;
            khachHang.Email = Email;
            khachHang.SoDienThoai = SoDienThoai;
            khachHang.DiaChi = DiaChi;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // KHÓA / MỞ KHÓA KHÁCH HÀNG
        // POST: Admin/KhachHangs/DoiTrangThai/5
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            // true  -> false
            // false -> true
            khachHang.TrangThai = !khachHang.TrangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // KIỂM TRA KHÁCH HÀNG CÓ TỒN TẠI
        // =====================================================
        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs
                .Any(kh => kh.MaKhachHang == id);
        }
    }
}