using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PhieuNhapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhieuNhapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách phiếu nhập
        public async Task<IActionResult> Index()
        {
            var phieuNhaps = await _context.PhieuNhaps
                .Include(p => p.NhaCungCap)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return View(phieuNhaps);
        }

        // Chi tiết phiếu nhập
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhaps
                .Include(p => p.NhaCungCap)
                .Include(p => p.ChiTietPhieuNhaps)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(p => p.MaPhieuNhap == id);

            if (phieuNhap == null)
            {
                return NotFound();
            }

            return View(phieuNhap);
        }

        // Mở trang tạo phiếu nhập
        public async Task<IActionResult> Create()
        {
            await LoadDanhSach();

            var viewModel = new PhieuNhapViewModel
            {
                NgayNhap = DateTime.Now,

                // Ban đầu cho sẵn 1 dòng sản phẩm
                ChiTietPhieuNhaps = new List<ChiTietPhieuNhapViewModel>
                {
                    new ChiTietPhieuNhapViewModel()
                }
            };

            return View(viewModel);
        }

        // Tạo phiếu nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhieuNhapViewModel model)
        {
            // Loại bỏ những dòng không có dữ liệu hợp lệ
            model.ChiTietPhieuNhaps ??=
                new List<ChiTietPhieuNhapViewModel>();

            if (model.ChiTietPhieuNhaps.Count == 0)
            {
                ModelState.AddModelError(
                    "",
                    "Phiếu nhập phải có ít nhất một sản phẩm."
                );
            }

            // Kiểm tra nhà cung cấp có tồn tại không
            var nhaCungCap = await _context.NhaCungCaps
                .FirstOrDefaultAsync(n =>
                    n.MaNhaCungCap == model.MaNhaCungCap &&
                    n.TrangThai);

            if (nhaCungCap == null)
            {
                ModelState.AddModelError(
                    "MaNhaCungCap",
                    "Nhà cung cấp không hợp lệ hoặc đã ngừng hoạt động."
                );
            }

            // Không cho chọn trùng sản phẩm trong cùng phiếu
            var sanPhamBiTrung = model.ChiTietPhieuNhaps
                .GroupBy(x => x.MaSanPham)
                .Any(g => g.Count() > 1);

            if (sanPhamBiTrung)
            {
                ModelState.AddModelError(
                    "",
                    "Một sản phẩm không được xuất hiện nhiều lần trong cùng phiếu nhập."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadDanhSach();
                return View(model);
            }

            // Transaction giúp tránh trường hợp:
            // đã tạo phiếu nhưng chưa kịp cộng tồn kho
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                decimal tongTien = 0;

                var phieuNhap = new PhieuNhap
                {
                    MaNhaCungCap = model.MaNhaCungCap,
                    NgayNhap = model.NgayNhap,
                    GhiChu = model.GhiChu,
                    TongTien = 0
                };

                _context.PhieuNhaps.Add(phieuNhap);
                await _context.SaveChangesAsync();

                foreach (var item in model.ChiTietPhieuNhaps)
                {
                    var sanPham = await _context.SanPhams
                        .FirstOrDefaultAsync(s =>
                            s.MaSanPham == item.MaSanPham);

                    if (sanPham == null)
                    {
                        throw new Exception(
                            "Có sản phẩm không tồn tại trong hệ thống."
                        );
                    }

                    decimal thanhTien =
                        item.SoLuong * item.GiaNhap;

                    tongTien += thanhTien;

                    var chiTiet = new ChiTietPhieuNhap
                    {
                        MaPhieuNhap = phieuNhap.MaPhieuNhap,
                        MaSanPham = item.MaSanPham,
                        SoLuong = item.SoLuong,
                        GiaNhap = item.GiaNhap
                    };

                    _context.ChiTietPhieuNhaps.Add(chiTiet);

                    // Cộng số lượng tồn
                    sanPham.SoLuongTon += item.SoLuong;
                }

                phieuNhap.TongTien = tongTien;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] =
                    "Nhập kho thành công.";

                return RedirectToAction(nameof(Details),
                    new { id = phieuNhap.MaPhieuNhap });
            }
            catch
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError(
                    "",
                    "Không thể tạo phiếu nhập. Vui lòng thử lại."
                );

                await LoadDanhSach();

                return View(model);
            }
        }

        // Load dropdown Nhà cung cấp và Sản phẩm
        private async Task LoadDanhSach()
        {
            var nhaCungCaps = await _context.NhaCungCaps
                .Where(n => n.TrangThai)
                .OrderBy(n => n.TenNhaCungCap)
                .ToListAsync();

            var sanPhams = await _context.SanPhams
                .Where(s => s.TrangThai)
                .OrderBy(s => s.TenSanPham)
                .ToListAsync();

            ViewBag.NhaCungCaps = new SelectList(
                nhaCungCaps,
                "MaNhaCungCap",
                "TenNhaCungCap"
            );

            ViewBag.SanPhams = new SelectList(
                sanPhams,
                "MaSanPham",
                "TenSanPham"
            );
        }
    }
}