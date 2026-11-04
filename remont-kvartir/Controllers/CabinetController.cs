using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;
using remont_kvartir.Models;
using remont_kvartir.ViewModels;

namespace remont_kvartir.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CabinetController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var requests = await _db.RepairRequests
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var model = new CabinetViewModel
            {
                User = user,
                Requests = requests,
                TotalRequests = requests.Count,
                NewRequests = requests.Count(r => r.Status == RequestStatus.New),
                InProgressRequests = requests.Count(r => r.Status == RequestStatus.InProgress),
                CompletedRequests = requests.Count(r => r.Status == RequestStatus.Completed)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Phone = user.Phone
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            user.FullName = model.FullName;
            user.Phone = model.Phone;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Профиль обновлён";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["Success"] = "Пароль изменён";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View(new RepairRequestViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequest(RepairRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var request = new RepairRequest
            {
                UserId = user.Id,
                RepairType = model.RepairType,
                Description = model.Description,
                Address = model.Address,
                Area = model.Area,
                Status = RequestStatus.New
            };

            _db.RepairRequests.Add(request);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Заявка успешно создана";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RequestDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var request = await _db.RepairRequests
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var request = await _db.RepairRequests
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);

            if (request == null)
                return NotFound();

            if (request.Status == RequestStatus.New)
            {
                request.Status = RequestStatus.Cancelled;
                request.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                TempData["Success"] = "Заявка отменена";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}