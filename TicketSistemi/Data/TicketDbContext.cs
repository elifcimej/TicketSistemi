using Microsoft.EntityFrameworkCore;
using TicketSistemi.Models;

namespace TicketSistemi.Data
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketTimetable> TicketTimelines { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Personel" },
                new Role { Id = 3, Name = "Takım Lideri" },
                new Role { Id = 4, Name = "Müşteri" }
            );
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Elif", Email = "musteri@gmail.com", PasswordHash = "1234", RoleId = 3 },
                new User { Id = 2, FullName = "Ayşe", Email = "personel@gmail.com", PasswordHash = "1234", RoleId = 2 },
                new User { Id = 3, FullName = "Admin", Email = "admin@gmail.com", PasswordHash = "1234", RoleId = 1 }
            );

            modelBuilder.Entity<TicketComment>()
                .HasOne(tc => tc.CreatedBy)
                .WithMany()
                .HasForeignKey(tc => tc.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketComment>()
                .HasOne(tc => tc.Ticket)
                .WithMany()
                .HasForeignKey(tc => tc.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketTimetable>()
                .HasOne(tl => tl.PerformedBy)
                .WithMany()
                .HasForeignKey(tl => tl.PerformedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketTimetable>()
                .HasOne(tl => tl.Ticket)
                .WithMany()
                .HasForeignKey(tl => tl.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

        }

    }
}
