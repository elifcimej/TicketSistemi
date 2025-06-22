using Microsoft.AspNetCore.Mvc;

namespace TicketUI.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
