namespace TicketSistemi.Models
{
    public class TicketTimetable
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Action { get; set; } // Örn: "Statü değişti"
        public int? PerformedById { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        public Ticket Ticket { get; set; }
        public User PerformedBy { get; set; }
    }
}
