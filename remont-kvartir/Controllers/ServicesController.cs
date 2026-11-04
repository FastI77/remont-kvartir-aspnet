using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;
using remont_kvartir.Models;

namespace remont_kvartir.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search, string? sort, string? category)
        {
            var query = _db.Services.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(s => s.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Title.Contains(search));
            }

            switch (sort)
            {
                case "price_asc":
                    query = query.OrderBy(s => string.IsNullOrEmpty(s.Price) ? "яяя" : s.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(s => s.Price);
                    break;
                case "name_asc":
                    query = query.OrderBy(s => s.Title);
                    break;
                default:
                    query = query.OrderByDescending(s => s.Id);
                    break;
            }

            ViewBag.Categories = await _db.Services
                .Where(s => !string.IsNullOrEmpty(s.Category))
                .Select(s => s.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentCategory = category;

            var services = await query.ToListAsync();
            return View(services);
        }
        public async Task<IActionResult> Details(int id)
        {
            var service = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }
    }
}