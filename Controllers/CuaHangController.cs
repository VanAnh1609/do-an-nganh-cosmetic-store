using CosmeticStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CuaHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sanPhams = await _context.SanPhams
                .Where(sp => sp.TrangThai)
                .ToListAsync();

            return View(sanPhams);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp =>
                    sp.MaSanPham == id &&
                    sp.TrangThai);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }
}