using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSistemi.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }   
        public string Comment { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Ticket Ticket { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; }
    }
}
