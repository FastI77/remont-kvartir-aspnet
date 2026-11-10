using Microsoft.AspNetCore.Mvc;

namespace remont_kvartir.Controllers
{
    public class ContactMessageDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage([FromBody] ContactMessageDto data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Message))
            {
                return Json(new { success = false, message = "Пожалуйста, заполните все поля." });
            }

            return Json(new { success = true, message = "Спасибо! Ваше сообщение отправлено. Мы свяжемся с вами в ближайшее время." });
        }
    }
}