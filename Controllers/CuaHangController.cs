using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CuaHangController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Danh sách sản phẩm phía khách hàng
            public async Task<IActionResult> Index(
             string? tuKhoa,
             int? maDanhMuc,
             int? maThuongHieu,
             string? sapXep)
        {
            var query = _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.ThuongHieu)
                .Where(sp => sp.TrangThai)
                .AsQueryable();

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                query = query.Where(sp =>
                    sp.TenSanPham.Contains(tuKhoa));
            }

            // Lọc theo danh mục
            if (maDanhMuc.HasValue)
            {
                query = query.Where(sp =>
                    sp.MaDanhMuc == maDanhMuc.Value);
            }

            // Lọc theo thương hiệu
            if (maThuongHieu.HasValue)
            {
                query = query.Where(sp =>
                    sp.MaThuongHieu == maThuongHieu.Value);
            }

            // Sắp xếp
            query = sapXep switch
            {
                "giaTang" => query
                    .OrderBy(sp => sp.GiaBan),

                "giaGiam" => query
                    .OrderByDescending(sp => sp.GiaBan),

                "tenAZ" => query
                    .OrderBy(sp => sp.TenSanPham.Trim()),

                "tenZA" => query
                    .OrderByDescending(sp => sp.TenSanPham.Trim()),

                _ => query
                    .OrderByDescending(sp => sp.NgayTao)
            };

            var sanPhams = await query.ToListAsync();
            var sanPhamDaYeuThich = new List<int>();

            if (User.Identity?.IsAuthenticated == true)
            {
                var identityUser =
                    await _userManager.GetUserAsync(User);

                if (identityUser?.Email != null)
                {
                    var khachHang = await _context.KhachHangs
                        .AsNoTracking()
                        .FirstOrDefaultAsync(kh =>
                            kh.Email == identityUser.Email);

                    if (khachHang != null)
                    {
                        sanPhamDaYeuThich = await _context.YeuThichs
                            .AsNoTracking()
                            .Where(yt =>
                                yt.MaKhachHang == khachHang.MaKhachHang)
                            .Select(yt => yt.MaSanPham)
                            .ToListAsync();
                    }
                }
            }

            ViewBag.SanPhamDaYeuThich = sanPhamDaYeuThich;

            // Load dropdown danh mục
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs
                    .Where(dm => dm.TrangThai)
                    .OrderBy(dm => dm.TenDanhMuc)
                    .ToListAsync(),
                "MaDanhMuc",
                "TenDanhMuc",
                maDanhMuc);

            // Load dropdown thương hiệu
            ViewBag.ThuongHieus = new SelectList(
                await _context.ThuongHieus
                    .Where(th => th.TrangThai)
                    .OrderBy(th => th.TenThuongHieu)
                    .ToListAsync(),
                "MaThuongHieu",
                "TenThuongHieu",
                maThuongHieu);

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.MaThuongHieu = maThuongHieu;
            ViewBag.SapXep = sapXep;

            return View(sanPhams);
        }

        // SẢN PHẨM BÁN CHẠY
        public async Task<IActionResult> BanChay()
        {
            // Tính tổng số lượng đã bán của từng sản phẩm
            var soLuongDaBan = await _context.ChiTietDonHangs
                .GroupBy(ct => ct.MaSanPham)
                .Select(g => new
                {
                    MaSanPham = g.Key,
                    SoLuongDaBan = g.Sum(ct => ct.SoLuong)
                })
                .ToDictionaryAsync(
                    x => x.MaSanPham,
                    x => x.SoLuongDaBan
                );

            // Lấy sản phẩm và xếp theo số lượng bán
            var sanPhams = await _context.SanPhams
                .AsNoTracking()
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.ThuongHieu)
                .Where(sp => sp.TrangThai)
                .OrderByDescending(sp =>
                    sp.ChiTietDonHangs.Sum(ct => (int?)ct.SoLuong) ?? 0)
                .ToListAsync();

            // Báo cho Index.cshtml biết đây là trang Bán chạy
            ViewBag.LaTrangBanChay = true;

            // Gửi số lượng đã bán sang View
            ViewBag.SoLuongDaBan = soLuongDaBan;

            // Danh mục
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs
                    .AsNoTracking()
                    .Where(dm => dm.TrangThai)
                    .OrderBy(dm => dm.TenDanhMuc)
                    .ToListAsync(),
                "MaDanhMuc",
                "TenDanhMuc"
            );

            // Thương hiệu
            ViewBag.ThuongHieus = new SelectList(
                await _context.ThuongHieus
                    .AsNoTracking()
                    .Where(th => th.TrangThai)
                    .OrderBy(th => th.TenThuongHieu)
                    .ToListAsync(),
                "MaThuongHieu",
                "TenThuongHieu"
            );

            ViewBag.TuKhoa = null;
            ViewBag.MaDanhMuc = null;
            ViewBag.MaThuongHieu = null;
            ViewBag.SapXep = null;

            return View("Index", sanPhams);
        }
        // Danh sách thương hiệu
        public async Task<IActionResult> ThuongHieu()
        {
            var thuongHieus = await _context.ThuongHieus
                .AsNoTracking()
                .Where(th => th.TrangThai)
                .OrderBy(th => th.TenThuongHieu)
                .ToListAsync();

            return View(thuongHieus);
        }
        // KHUYẾN MÃI
        public async Task<IActionResult> KhuyenMai()
        {
            var homNay = DateTime.Now;

            var maGiamGias = await _context.MaGiamGias
                .AsNoTracking()
                .Where(m =>
                    m.TrangThai &&
                    m.SoLuong > 0 &&
                    m.NgayBatDau <= homNay &&
                    m.NgayKetThuc >= homNay)
                .OrderByDescending(m => m.PhanTramGiam)
                .ThenBy(m => m.NgayKetThuc)
                .ToListAsync();

            return View(maGiamGias);
        }
        // Chi tiết sản phẩm
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhSanPhams)
                .Include(sp => sp.DanhGias)
                    .ThenInclude(dg => dg.KhachHang)
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == id &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DanhGiaSanPham(int id)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == id &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Kiểm tra khách đã mua sản phẩm và đơn đã giao
            bool daMuaSanPham = await _context.DonHangs
                .AnyAsync(dh =>
                    dh.MaKhachHang == khachHang.MaKhachHang &&
                    dh.TrangThai == "DaGiao" &&
                    dh.ChiTietDonHangs.Any(ct =>
                        ct.MaSanPham == id));

            if (!daMuaSanPham)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn chỉ có thể đánh giá sản phẩm đã mua và đã nhận hàng.";

                return RedirectToAction(
                    "DonHangCuaToi",
                    "GioHang");
            }

            // Nếu đã đánh giá rồi thì không cho đánh giá tiếp
            bool daDanhGia = await _context.DanhGias
                .AnyAsync(dg =>
                    dg.MaKhachHang == khachHang.MaKhachHang &&
                    dg.MaSanPham == id);

            if (daDanhGia)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn đã đánh giá sản phẩm này rồi.";

                return RedirectToAction(
                    "DonHangCuaToi",
                    "GioHang");
            }

            return View(sanPham);
        }

        // Khách hàng gửi đánh giá sản phẩm
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuiDanhGia(
            int maSanPham,
            int soSao,
            string? binhLuan)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            // Kiểm tra số sao
            if (soSao < 1 || soSao > 5)
            {
                TempData["ThongBaoLoi"] =
                    "Số sao phải từ 1 đến 5.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = maSanPham });
            }

            // Kiểm tra sản phẩm có tồn tại không
            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == maSanPham &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Chỉ khách đã mua và đơn đã giao mới được đánh giá
            bool daMuaSanPham = await _context.DonHangs
                .AnyAsync(dh =>
                    dh.MaKhachHang == khachHang.MaKhachHang &&
                    dh.TrangThai == "DaGiao" &&
                    dh.ChiTietDonHangs.Any(ct =>
                        ct.MaSanPham == maSanPham));

            if (!daMuaSanPham)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn chỉ có thể đánh giá sản phẩm đã mua và đã nhận hàng.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = maSanPham });
            }

            // Không cho cùng một khách đánh giá cùng sản phẩm nhiều lần
            bool daDanhGia = await _context.DanhGias
                .AnyAsync(dg =>
                    dg.MaKhachHang == khachHang.MaKhachHang &&
                    dg.MaSanPham == maSanPham);

            if (daDanhGia)
            {
                TempData["ThongBaoLoi"] =
                    "Bạn đã đánh giá sản phẩm này rồi.";

                return RedirectToAction(
                 nameof(DanhGiaSanPham),
                 new { id = maSanPham });
            }

            var danhGia = new DanhGia
            {
                MaKhachHang = khachHang.MaKhachHang,
                MaSanPham = maSanPham,
                SoSao = soSao,
                BinhLuan = string.IsNullOrWhiteSpace(binhLuan)
                    ? null
                    : binhLuan.Trim(),
                NgayDanhGia = DateTime.Now,
                DaDuyet = false
            };

            _context.DanhGias.Add(danhGia);

            await _context.SaveChangesAsync();

            TempData["ThongBaoThanhCong"] =
                "Đã gửi đánh giá. Đánh giá sẽ hiển thị sau khi được Admin duyệt.";

            return RedirectToAction(
            nameof(DanhGiaSanPham),
            new { id = maSanPham });
        }
        // Danh sách sản phẩm yêu thích
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DanhSachYeuThich()
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            var yeuThichs = await _context.YeuThichs
                .AsNoTracking()
                .Include(yt => yt.SanPham)
                .Where(yt =>
                    yt.MaKhachHang == khachHang.MaKhachHang &&
                    yt.SanPham != null &&
                    yt.SanPham.TrangThai)
                .OrderByDescending(yt => yt.NgayThem)
                .ToListAsync();

            return View(yeuThichs);
        }


        // Thêm sản phẩm vào yêu thích
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemYeuThich(
            int maSanPham,
            string? returnUrl)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == maSanPham &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            bool daYeuThich = await _context.YeuThichs
                .AnyAsync(yt =>
                    yt.MaKhachHang == khachHang.MaKhachHang &&
                    yt.MaSanPham == maSanPham);

            if (!daYeuThich)
            {
                var yeuThich = new YeuThich
                {
                    MaKhachHang = khachHang.MaKhachHang,
                    MaSanPham = maSanPham,
                    NgayThem = DateTime.Now
                };

                _context.YeuThichs.Add(yeuThich);

                await _context.SaveChangesAsync();

                TempData["ThongBaoThanhCong"] =
                    "Đã thêm sản phẩm vào danh sách yêu thích.";
            }
            else
            {
                TempData["ThongBaoLoi"] =
                    "Sản phẩm này đã có trong danh sách yêu thích.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }


        // Bỏ sản phẩm khỏi yêu thích
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoYeuThich(
            int maSanPham,
            string? returnUrl)
        {
            var identityUser =
                await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Challenge();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Forbid();
            }

            var yeuThich = await _context.YeuThichs
                .FirstOrDefaultAsync(yt =>
                    yt.MaKhachHang == khachHang.MaKhachHang &&
                    yt.MaSanPham == maSanPham);

            if (yeuThich != null)
            {
                _context.YeuThichs.Remove(yeuThich);

                await _context.SaveChangesAsync();

                TempData["ThongBaoThanhCong"] =
                    "Đã bỏ sản phẩm khỏi danh sách yêu thích.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(DanhSachYeuThich));
        }

        // Thêm yêu thích bằng AJAX
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemYeuThichAjax(int maSanPham)
        {
            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Unauthorized();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Unauthorized();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == maSanPham &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            bool daTonTai = await _context.YeuThichs
                .AnyAsync(yt =>
                    yt.MaKhachHang == khachHang.MaKhachHang &&
                    yt.MaSanPham == maSanPham);

            if (!daTonTai)
            {
                _context.YeuThichs.Add(new YeuThich
                {
                    MaKhachHang = khachHang.MaKhachHang,
                    MaSanPham = maSanPham,
                    NgayThem = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            return Ok();
        }


        // Bỏ yêu thích bằng AJAX
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoYeuThichAjax(int maSanPham)
        {
            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser?.Email == null)
            {
                return Unauthorized();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    kh.Email == identityUser.Email);

            if (khachHang == null)
            {
                return Unauthorized();
            }

            var yeuThich = await _context.YeuThichs
                .FirstOrDefaultAsync(yt =>
                    yt.MaKhachHang == khachHang.MaKhachHang &&
                    yt.MaSanPham == maSanPham);

            if (yeuThich != null)
            {
                _context.YeuThichs.Remove(yeuThich);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }



}