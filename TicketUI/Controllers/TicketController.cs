using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSistemi.Data;
using TicketSistemi.Models;

namespace TicketUI.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketDbContext _context;

        public TicketController(TicketDbContext context)
        {
            _context = context;
        }
        static List<Ticket> tickets = new List<Ticket>
        {
            new Ticket
            {
                Id = 1,
                Title = "Yazıcı arızası",
                Description = "Çıktı alamıyorum.",
                Priority = "Yüksek",
                Status = TicketStatus.Yeni,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                CreatedById = 1,
                AssignedToId = 2
            },
            new Ticket
            {
                Id = 2,
                Title = "WiFi problemi",
                Description = "Ağa bağlanamıyorum.",
                Priority = "Orta",
                Status = TicketStatus.Islemde,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                CreatedById = 2,
                AssignedToId = 2
            },
            new Ticket
            {
                Id = 3,
                Title = "Mouse çalışmıyor",
                Description = "Tıklamıyor.",
                Priority = "Düşük",
                Status = TicketStatus.Kapatildi,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CreatedById = 1,
                AssignedToId = null
            }
        };

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var userRole = HttpContext.Session.GetString("Role");

            if (userRole == "Müşteri")
            {
                var myTickets = tickets.Where(t => t.CreatedById == userId).ToList();
                return View(myTickets);
            }
            return View(tickets);
        }

        public IActionResult Assigned()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var userRole = HttpContext.Session.GetString("Role");

            if (userRole == "Personel")
            {
                var assignedTickets = tickets.Where(t => t.AssignedToId == userId).ToList();
                return View(assignedTickets);
            }

            return View(new List<Ticket>());
        }

        public IActionResult AdminPanel()
        {
            return View(tickets);
        }
  
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ticket ticket)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            ticket.CreatedById = userId.Value;
            ticket.CreatedAt = DateTime.Now;
            ticket.Status = TicketStatus.Yeni; 

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
