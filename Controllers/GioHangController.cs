using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CosmeticStore.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private const string GioHangKey = "GioHang";

        public GioHangController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lấy giỏ hàng từ Session
        private List<GioHangItem> LayGioHang()
        {
            var json = HttpContext.Session.GetString(GioHangKey);

            if (string.IsNullOrEmpty(json))
            {
                return new List<GioHangItem>();
            }

            return JsonSerializer.Deserialize<List<GioHangItem>>(json)
                   ?? new List<GioHangItem>();
        }

        // Lưu giỏ hàng vào Session
        private void LuuGioHang(List<GioHangItem> gioHang)
        {
            var json = JsonSerializer.Serialize(gioHang);

            HttpContext.Session.SetString(GioHangKey, json);
        }

        // GET: /GioHang
        public IActionResult Index()
        {
            return View(LayGioHang());
        }

        // Thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemVaoGio(
            int id,
            int soLuong = 1)
        {
            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == id &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            if (sanPham.SoLuongTon <= 0)
            {
                TempData["ThongBaoLoi"] =
                    "Sản phẩm hiện đã hết hàng.";

                return RedirectToAction(
                    "Details",
                    "CuaHang",
                    new { id });
            }

            soLuong = Math.Max(1, soLuong);

            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(
                gh => gh.MaSanPham == id);

            if (item == null)
            {
                gioHang.Add(new GioHangItem
                {
                    MaSanPham = sanPham.MaSanPham,
                    TenSanPham = sanPham.TenSanPham,
                    HinhAnh = sanPham.HinhAnh,
                    GiaBan = sanPham.GiaBan,
                    SoLuong = Math.Min(
                        soLuong,
                        sanPham.SoLuongTon)
                });
            }
            else
            {
                item.SoLuong = Math.Min(
                    item.SoLuong + soLuong,
                    sanPham.SoLuongTon);
            }

            LuuGioHang(gioHang);

            TempData["ThongBaoThanhCong"] =
                "Đã thêm sản phẩm vào giỏ hàng.";

            return RedirectToAction(nameof(Index));
        }

        // Tăng số lượng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TangSoLuong(int id)
        {
            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(
                gh => gh.MaSanPham == id);

            if (item == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
            {
                gioHang.Remove(item);
                LuuGioHang(gioHang);

                TempData["ThongBaoLoi"] =
                    "Sản phẩm không còn tồn tại.";
            }
            else if (item.SoLuong < sanPham.SoLuongTon)
            {
                item.SoLuong++;
                LuuGioHang(gioHang);
            }
            else
            {
                TempData["ThongBaoLoi"] =
                    "Số lượng đã đạt mức tồn kho.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Giảm số lượng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GiamSoLuong(int id)
        {
            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(
                gh => gh.MaSanPham == id);

            if (item != null)
            {
                if (item.SoLuong > 1)
                {
                    item.SoLuong--;
                }
                else
                {
                    gioHang.Remove(item);
                }

                LuuGioHang(gioHang);
            }

            return RedirectToAction(nameof(Index));
        }

        // Xóa một sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaSanPham(int id)
        {
            var gioHang = LayGioHang();

            var item = gioHang.FirstOrDefault(
                gh => gh.MaSanPham == id);

            if (item != null)
            {
                gioHang.Remove(item);
                LuuGioHang(gioHang);
            }

            return RedirectToAction(nameof(Index));
        }

        // Xóa toàn bộ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaTatCa()
        {
            HttpContext.Session.Remove(GioHangKey);

            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form thanh toán
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ThanhToan()
        {
            var gioHang = LayGioHang();

            if (!gioHang.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            var model = new ThanhToanViewModel
            {
                TenNguoiNhan = khachHang?.HoTen ?? string.Empty,
                SoDienThoai = khachHang?.SoDienThoai ?? string.Empty,
                DiaChiGiaoHang = khachHang?.DiaChi ?? string.Empty,
                PhuongThucThanhToan = "COD"
            };

            ViewBag.TongTien =
                gioHang.Sum(item => item.ThanhTien);

            return View(model);
        }

        // Xử lý đặt hàng
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToan(
            ThanhToanViewModel model)
        {
            var gioHang = LayGioHang();

            if (!gioHang.Any())
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Giỏ hàng đang trống.");
            }

            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            ViewBag.TongTien =
                gioHang.Sum(item => item.ThanhTien);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var khachHang = await _context.KhachHangs
                    .FirstOrDefaultAsync(kh =>
                        kh.Email == identityUser.Email);

                if (khachHang == null)
                {
                    khachHang = new KhachHang
                    {
                        HoTen = model.TenNguoiNhan,
                        Email = identityUser.Email,
                        MatKhauHash =
                            identityUser.PasswordHash
                            ?? "IDENTITY_ACCOUNT",
                        SoDienThoai = model.SoDienThoai,
                        DiaChi = model.DiaChiGiaoHang,
                        VaiTro = "KhachHang",
                        NgayDangKy = DateTime.Now,
                        TrangThai = true
                    };

                    _context.KhachHangs.Add(khachHang);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    khachHang.HoTen =
                        model.TenNguoiNhan;

                    khachHang.SoDienThoai =
                        model.SoDienThoai;

                    khachHang.DiaChi =
                        model.DiaChiGiaoHang;
                }

                decimal tongTien =
                    gioHang.Sum(item => item.ThanhTien);

                var donHang = new DonHang
                {
                    MaKhachHang = khachHang.MaKhachHang,
                    MaGiamGiaId = null,
                    TenNguoiNhan = model.TenNguoiNhan,
                    SoDienThoai = model.SoDienThoai,
                    DiaChiGiaoHang = model.DiaChiGiaoHang,
                    NgayDat = DateTime.Now,
                    TongTien = tongTien,
                    TienGiam = 0,
                    PhiVanChuyen = 0,
                    TrangThai = "ChoXacNhan",
                    PhuongThucThanhToan =
                        model.PhuongThucThanhToan,
                    GhiChu = model.GhiChu
                };

                _context.DonHangs.Add(donHang);

                await _context.SaveChangesAsync();

                foreach (var item in gioHang)
                {
                    var sanPham = await _context.SanPhams
                        .FirstOrDefaultAsync(sp =>
                            sp.MaSanPham == item.MaSanPham);

                    if (sanPham == null ||
                        !sanPham.TrangThai)
                    {
                        throw new InvalidOperationException(
                            $"Sản phẩm {item.TenSanPham} không còn tồn tại.");
                    }

                    if (sanPham.SoLuongTon < item.SoLuong)
                    {
                        throw new InvalidOperationException(
                            $"Sản phẩm {item.TenSanPham} không đủ số lượng.");
                    }

                    var chiTiet = new ChiTietDonHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaSanPham = item.MaSanPham,
                        SoLuong = item.SoLuong,
                        DonGia = item.GiaBan
                    };

                    _context.ChiTietDonHangs.Add(chiTiet);

                    sanPham.SoLuongTon -= item.SoLuong;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                HttpContext.Session.Remove(GioHangKey);

                return RedirectToAction(
                    nameof(DatHangThanhCong),
                    new { id = donHang.MaDonHang });
            }
            catch (InvalidOperationException ex)
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(model);
            }
            catch
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "Không thể đặt hàng. Vui lòng thử lại.");

                return View(model);
            }
        }

        // Danh sách đơn hàng của Customer đang đăng nhập
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DonHangCuaToi()
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var donHangs = await _context.DonHangs
                .AsNoTracking()
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Where(dh =>
                    dh.KhachHang != null &&
                    dh.KhachHang.Email == identityUser.Email)
                .OrderByDescending(dh => dh.NgayDat)
                .ToListAsync();

            return View(donHangs);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DatHangThanhCong(int id)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var donHang = await _context.DonHangs
                .AsNoTracking()
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(dh =>
                    dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") &&
                donHang.KhachHang?.Email != identityUser.Email)
            {
                return Forbid();
            }

            return View(donHang);
        }
    }
}