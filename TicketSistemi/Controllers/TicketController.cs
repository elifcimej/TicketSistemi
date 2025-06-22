using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSistemi.Data;
using TicketSistemi.Models;

namespace TicketSistemi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketDbContext _context;
        public TicketController(TicketDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToList();
            return Ok(tickets);
        }

        [HttpGet("mine/{userId}")] //müşterinin açtığı
        public IActionResult GetMyTickets(int userId)
        {
            var tickets = _context.Tickets
                .Where(t => t.CreatedById == userId)
                .ToList();
            return Ok(tickets);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Ticket ticket)
        {
            ticket.Status = TicketStatus.Yeni;
            ticket.CreatedAt = DateTime.UtcNow;
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
            return Ok(ticket);
        }

        [HttpPut("{id}/status")] //statü değiştirme
        public IActionResult UpdateStatus(int id, [FromBody] TicketStatus newStatus)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null) return NotFound();

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            // BU TRAKİBİ DİĞER İŞLEMLER İÇİN DE YAPACAĞIM AMA SONRA UNUTMA
            var timeline = new TicketTimetable
            {
                TicketId = ticket.Id,
                Action = $"Statü {newStatus} olarak değiştirildi.",
                PerformedById = 1, // (auth yok düzelteceğim)
                ActionDate = DateTime.UtcNow
            };
            _context.TicketTimelines.Add(timeline);
            _context.SaveChanges();

            return Ok(ticket);

        }

        [HttpPut("{id}/assign")] //devretme
        public IActionResult Reassign(int id, [FromBody] int newAssigneeId)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null) return NotFound();

            ticket.AssignedToId = newAssigneeId;
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return Ok(ticket);
        }

        [HttpGet("assigned/{userId}")]
        public IActionResult GetAssignedTickets(int userId)
        {
            var tickets = _context.Tickets
                .Where(t => t.AssignedToId == userId)
                .ToList();
            return Ok(tickets);
        }

        [HttpPost("{ticketId}/comment")]
        public IActionResult AddComment(int ticketId, [FromBody] string commentText)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket == null) return NotFound();

            var comment = new TicketComment
            {
                TicketId = ticketId,
                Comment = commentText,
                CreatedById = 1
            };
            _context.TicketComments.Add(comment);
            _context.SaveChanges();
            return Ok(comment);
        }

        [HttpPost("{ticketId}/upload")]
        public async Task<IActionResult> UploadAttachment(int ticketId, IFormFile file)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket == null) return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxFileSize)
                return BadRequest("Dosya boyutu 5 MB'ı aşamaz.");

            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new TicketAttachment
            {
                TicketId = ticketId,
                FileName = file.FileName,
                FilePath = fileName
            };
            _context.TicketAttachments.Add(attachment);
            _context.SaveChanges();

            return Ok(new { attachment.Id, attachment.FileName, attachment.FilePath });
        }

    }
}
