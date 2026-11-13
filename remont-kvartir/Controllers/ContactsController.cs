using Microsoft.AspNetCore.Mvc;
using remont_kvartir.Data;
using remont_kvartir.Models;
using remont_kvartir.ViewModels;

namespace remont_kvartir.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage([FromBody] ContactViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new { success = false, message = "Пожалуйста, проверьте правильность заполнения полей." });
            }

            var contactMessage = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message,
                CreatedAt = DateTime.UtcNow
            };

            _db.ContactMessages.Add(contactMessage);
            _db.SaveChanges();

            return Json(new { success = true, message = "Спасибо! Ваше сообщение отправлено. Мы свяжемся с вами в ближайшее время." });
        }
    }
}