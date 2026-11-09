using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Data;
using remont_kvartir.Models;

namespace remont_kvartir.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async Task<string?> SaveFile(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/" + fileName;
        }

        // --- ГЛАВНАЯ (ДАШБОРД) ---
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalRequests = await _db.RepairRequests.CountAsync();
            ViewBag.NewRequests = await _db.RepairRequests.CountAsync(r => r.Status == RequestStatus.New);
            ViewBag.TotalServices = await _db.Services.CountAsync();
            ViewBag.TotalPortfolio = await _db.PortfolioItems.CountAsync();
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            return View();
        }

        // --- ПОЛЬЗОВАТЕЛИ ---
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }

            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            if (!string.IsNullOrEmpty(newRole))
            {
                await _userManager.AddToRoleAsync(user, newRole);
            }

            TempData["Success"] = $"Роль пользователя {user.Email} обновлена на {newRole}.";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == userId)
            {
                TempData["Error"] = "Вы не можете удалить свой собственный аккаунт.";
                return RedirectToAction(nameof(Users));
            }

            var requests = await _db.RepairRequests.Where(r => r.UserId == userId).ToListAsync();
            _db.RepairRequests.RemoveRange(requests);
            await _db.SaveChangesAsync();

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "Пользователь успешно удален.";
            return RedirectToAction(nameof(Users));
        }

        // --- ЗАЯВКИ ---
        public async Task<IActionResult> Requests()
        {
            var requests = await _db.RepairRequests
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRequestStatus(int id, RequestStatus status, string? returnUrl = null)
        {
            var request = await _db.RepairRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = status;
                request.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Статус заявки №{id} обновлен.";
            }
            else
            {
                TempData["Error"] = "Заявка не найдена.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Requests));
        }

        // --- УСЛУГИ ---
        public async Task<IActionResult> Services()
        {
            var services = await _db.Services.ToListAsync();
            return View(services);
        }

        public IActionResult ServiceCreate()
        {
            return View(new Service());
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCreate(Service model)
        {
            if (ModelState.IsValid)
            {
                _db.Services.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            return View(model);
        }

        public async Task<IActionResult> ServiceEdit(int id)
        {
            var service = await _db.Services.FindAsync(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> ServiceEdit(Service model)
        {
            if (ModelState.IsValid)
            {
                _db.Services.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ServiceDelete(int id)
        {
            var service = await _db.Services.FindAsync(id);
            if (service != null)
            {
                _db.Services.Remove(service);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Services));
        }

        // --- ПОРТФОЛИО ---
        public async Task<IActionResult> Portfolio()
        {
            var items = await _db.PortfolioItems.Include(p => p.Gallery).OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(items);
        }

        public IActionResult PortfolioCreate()
        {
            return View(new PortfolioItem());
        }

        [HttpPost]
        public async Task<IActionResult> PortfolioCreate(PortfolioItem model, IFormFile? mainImage, List<IFormFile>? galleryImages)
        {
            if (ModelState.IsValid)
            {
                if (mainImage != null) model.ImageUrl = await SaveFile(mainImage);

                _db.PortfolioItems.Add(model);
                await _db.SaveChangesAsync();

                if (galleryImages != null && galleryImages.Count > 0)
                {
                    foreach (var file in galleryImages)
                    {
                        var path = await SaveFile(file);
                        if (path != null)
                        {
                            _db.PortfolioImages.Add(new PortfolioImage { ImageUrl = path, PortfolioItemId = model.Id });
                        }
                    }
                    await _db.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Portfolio));
            }
            return View(model);
        }

        public async Task<IActionResult> PortfolioEdit(int id)
        {
            var item = await _db.PortfolioItems.Include(p => p.Gallery).FirstOrDefaultAsync(p => p.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> PortfolioEdit(PortfolioItem model, IFormFile? mainImage, List<IFormFile>? galleryImages)
        {
            if (ModelState.IsValid)
            {
                if (mainImage != null) model.ImageUrl = await SaveFile(mainImage);

                _db.PortfolioItems.Update(model);

                if (galleryImages != null && galleryImages.Count > 0)
                {
                    foreach (var file in galleryImages)
                    {
                        var path = await SaveFile(file);
                        if (path != null)
                        {
                            _db.PortfolioImages.Add(new PortfolioImage { ImageUrl = path, PortfolioItemId = model.Id });
                        }
                    }
                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Portfolio));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PortfolioDelete(int id)
        {
            var item = await _db.PortfolioItems.Include(p => p.Gallery).FirstOrDefaultAsync(p => p.Id == id);
            if (item != null)
            {
                _db.PortfolioItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Portfolio));
        }
    }
}