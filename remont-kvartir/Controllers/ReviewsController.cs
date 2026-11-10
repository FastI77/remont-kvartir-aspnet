using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;
using remont_kvartir.Models;
using remont_kvartir.ViewModels;

namespace remont_kvartir.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _db.Reviews
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(reviews);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();


            var completedRequests = await _db.RepairRequests
                .Where(r => r.UserId == user.Id && r.Status == RequestStatus.Completed)
                .Where(r => !_db.Reviews.Any(rev => rev.RepairRequestId == r.Id))
                .ToListAsync();

            if (!completedRequests.Any())
            {
                TempData["Error"] = "Вы можете оставить отзыв только после завершения ремонта (и только один раз на одну заявку).";
                return RedirectToAction(nameof(Index));
            }

            var model = new ReviewCreateViewModel
            {
                AvailableRequests = completedRequests
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewCreateViewModel model, IFormFile? imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                model.AvailableRequests = await _db.RepairRequests
                    .Where(r => r.UserId == user.Id && r.Status == RequestStatus.Completed)
                    .Where(r => !_db.Reviews.Any(rev => rev.RepairRequestId == r.Id))
                    .ToListAsync();
                return View(model);
            }

            string? imageUrl = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                imageUrl = "/uploads/" + fileName;
            }

            var review = new Review
            {
                UserId = user.Id,
                AuthorName = user.FullName,
                RepairRequestId = model.RepairRequestId,
                Rating = model.Rating,
                Text = model.Text,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false
            };

            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Спасибо! Ваш отзыв отправлен на модерацию.";
            return RedirectToAction(nameof(Index));
        }
    }
}