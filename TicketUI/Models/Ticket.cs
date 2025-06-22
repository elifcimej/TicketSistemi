using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketUI.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


        public int CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; }

        public int? AssignedToId { get; set; }
        [ForeignKey(nameof(AssignedToId))]
        public User AssignedTo { get; set; }
    }
}
