using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;

namespace remont_kvartir.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PromotionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var promotions = await _db.Promotions
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(promotions);
        }
    }
}