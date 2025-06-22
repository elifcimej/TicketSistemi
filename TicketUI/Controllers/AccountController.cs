using Microsoft.AspNetCore.Mvc;

namespace TicketUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Test için, sadece "elif@gmail.com" ve "1234" ile giriş kabul ediyoruz
            if (email == "elif@gmail.com" && password == "1234")
            {
                // Başarılı login → Ticket listesine yönlendir
                return RedirectToAction("Index", "Ticket");
            }

            ViewBag.Error = "E-posta veya şifre yanlış!";
            return View();
        }
    }
}
