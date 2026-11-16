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
                .Include(r => r.Gallery)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(reviews);
        }

        public async Task<IActionResult> Details(int id)
        {
            var review = await _db.Reviews
                .Include(r => r.User)
                .Include(r => r.Gallery)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsApproved);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
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
        public async Task<IActionResult> Create(ReviewCreateViewModel model)
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

            var request = await _db.RepairRequests
                .FirstOrDefaultAsync(r => r.Id == model.RepairRequestId
                                       && r.UserId == user.Id
                                       && r.Status == RequestStatus.Completed);

            if (request == null)
            {
                ModelState.AddModelError("", "Можно оставить отзыв только по завершённой заявке.");
                model.AvailableRequests = await _db.RepairRequests
                    .Where(r => r.UserId == user.Id && r.Status == RequestStatus.Completed)
                    .Where(r => !_db.Reviews.Any(rev => rev.RepairRequestId == r.Id))
                    .ToListAsync();
                return View(model);
            }

            bool alreadyReviewed = await _db.Reviews.AnyAsync(r => r.RepairRequestId == model.RepairRequestId);
            if (alreadyReviewed)
            {
                ModelState.AddModelError("", "Вы уже оставили отзыв по этой заявке.");
                model.AvailableRequests = await _db.RepairRequests
                    .Where(r => r.UserId == user.Id && r.Status == RequestStatus.Completed)
                    .Where(r => !_db.Reviews.Any(rev => rev.RepairRequestId == r.Id))
                    .ToListAsync();
                return View(model);
            }

            var review = new Review
            {
                UserId = user.Id,
                AuthorName = user.FullName,
                RepairRequestId = model.RepairRequestId,
                Rating = model.Rating,
                Text = model.Text,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false
            };

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(folderPath);

                var files = model.ImageFiles.Take(5).ToList();

                foreach (var file in files)
                {
                    if (file.Length == 0) continue;

                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                    if (!allowed.Contains(ext)) continue;

                    var fileName = Guid.NewGuid().ToString() + ext;
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    review.Gallery.Add(new ReviewImage
                    {
                        ImageUrl = "/uploads/" + fileName
                    });
                }
            }

            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Спасибо! Ваш отзыв отправлен на модерацию.";
            return RedirectToAction(nameof(Index));
        }
    }
}