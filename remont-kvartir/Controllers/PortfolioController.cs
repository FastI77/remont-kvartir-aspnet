using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;

namespace remont_kvartir.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PortfolioController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.PortfolioItems
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.PortfolioItems
                .Include(p => p.Gallery)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (item == null) return NotFound();

            return View(item);
        }
    }
}