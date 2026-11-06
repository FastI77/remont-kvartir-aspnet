using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;
using remont_kvartir.Models;

namespace remont_kvartir.Controllers
{
    [Authorize(Roles = "Admin,Manager")] // Доступ только для админа и менеджера
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Список всех заявок
        public async Task<IActionResult> ManageRequests()
        {
            var requests = await _db.RepairRequests
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(requests);
        }

        // Смена статуса заявки
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, RequestStatus status)
        {
            var request = await _db.RepairRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = status;
                request.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageRequests));
        }

        // Список всех отзывов
        public async Task<IActionResult> ManageReviews()
        {
            var reviews = await _db.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(reviews);
        }

        // Удаление отзыва
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review != null)
            {
                _db.Reviews.Remove(review);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageReviews));
        }
    }
}