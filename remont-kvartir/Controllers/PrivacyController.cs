using Microsoft.AspNetCore.Mvc;

namespace remont_kvartir.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}