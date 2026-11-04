using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;
using remont_kvartir.Models;
using remont_kvartir.ViewModels;
using System.Diagnostics;

namespace remont_kvartir.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var latestServices = await _db.Services
                .OrderByDescending(s => s.Id)
                .Take(3)
                .ToListAsync();

            return View(latestServices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRequest([FromBody] HomeRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Пожалуйста, проверьте правильность заполнения полей." });
            }

            var request = new RepairRequest
            {
                RepairType = model.RepairType,
                Description = "Заявка оставлена с главной страницы",
                ClientName = model.Name,
                ClientPhone = model.Phone,
                Status = RequestStatus.New,
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null
            };

            _db.RepairRequests.Add(request);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Спасибо! Ваша заявка принята. Мы свяжемся с вами в ближайшее время." });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}