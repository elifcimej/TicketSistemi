using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using TicketSistemi.Data;           
using TicketSistemi.Models;         

namespace TicketUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly TicketDbContext _context;
        public AccountController(TicketDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Role", user.Role.Name);

                if (user.Role.Name == "Müşteri")
                    return RedirectToAction("Index", "Ticket");
                else if (user.Role.Name == "Personel")
                    return RedirectToAction("Assigned", "Ticket");
                else if (user.Role.Name == "Admin")
                    return RedirectToAction("AdminPanel", "Ticket");
            }

            ViewBag.Error = "E-posta veya şifre yanlış!";
            return View();
        }
    }
}
